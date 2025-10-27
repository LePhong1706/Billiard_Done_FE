using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class VaiTroNguoiDungRepository : GenericRepository<VaiTroNguoiDung>, IVaiTroNguoiDungRepository
{
    public VaiTroNguoiDungRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<VaiTroNguoiDung?> GetByTenVaiTroAsync(string tenVaiTro, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.TenVaiTro == tenVaiTro, cancellationToken);
    }

    public async Task<IEnumerable<VaiTroNguoiDung>> GetActiveRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .OrderBy(x => x.TenVaiTro)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByTenVaiTroAsync(string tenVaiTro, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TenVaiTro == tenVaiTro);
        
        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<int> CountUsersByRoleAsync(int vaiTroId, CancellationToken cancellationToken = default)
    {
        return await _context.NguoiDungs
            .CountAsync(x => x.MaVaiTro == vaiTroId, cancellationToken);
    }

    public async Task<bool> UpdateActiveStatusAsync(int vaiTroId, bool trangThaiHoatDong, CancellationToken cancellationToken = default)
    {
        var vaiTro = await GetByIdAsync(vaiTroId, cancellationToken);
        if (vaiTro == null)
            return false;

        vaiTro.TrangThaiHoatDong = trangThaiHoatDong;
        Update(vaiTro);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<(VaiTroNguoiDung VaiTro, int SoLuongNguoiDung)>> GetRolesWithUserCountAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbSet
            .GroupJoin(
                _context.NguoiDungs,
                vaiTro => vaiTro.Id,
                nguoiDung => nguoiDung.MaVaiTro,
                (vaiTro, nguoiDungs) => new { VaiTro = vaiTro, NguoiDungs = nguoiDungs }
            )
            .Select(x => new
            {
                VaiTro = x.VaiTro,
                SoLuongNguoiDung = x.NguoiDungs.Count()
            })
            .OrderBy(x => x.VaiTro.TenVaiTro)
            .ToListAsync(cancellationToken);

        return result.Select(x => (x.VaiTro, x.SoLuongNguoiDung));
    }
}