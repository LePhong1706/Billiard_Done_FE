using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class NhaCungCapRepository : GenericRepository<NhaCungCap>, INhaCungCapRepository
{
    public NhaCungCapRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<NhaCungCap?> GetByNameAsync(string tenNhaCungCap, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.TenNhaCungCap == tenNhaCungCap, cancellationToken);
    }

    public async Task<IEnumerable<NhaCungCap>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .OrderBy(x => x.TenNhaCungCap)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<NhaCungCap>> GetByCountryAsync(string quocGia, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.QuocGia == quocGia && x.TrangThaiHoatDong)
            .OrderBy(x => x.TenNhaCungCap)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string tenNhaCungCap, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TenNhaCungCap == tenNhaCungCap);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.Email == email);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<(IEnumerable<NhaCungCap> Suppliers, int TotalCount)> SearchAsync(
        string? searchTerm = null,
        string? quocGia = null,
        bool? trangThaiHoatDong = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(x =>
                x.TenNhaCungCap.ToLower().Contains(searchLower) ||
                (x.NguoiLienHe != null && x.NguoiLienHe.ToLower().Contains(searchLower)) ||
                (x.Email != null && x.Email.ToLower().Contains(searchLower)) ||
                (x.SoDienThoai != null && x.SoDienThoai.Contains(searchTerm)) ||
                (x.DiaChi != null && x.DiaChi.ToLower().Contains(searchLower)) ||
                (x.ThanhPho != null && x.ThanhPho.ToLower().Contains(searchLower))
            );
        }

        if (!string.IsNullOrWhiteSpace(quocGia))
        {
            query = query.Where(x => x.QuocGia == quocGia);
        }

        if (trangThaiHoatDong.HasValue)
        {
            query = query.Where(x => x.TrangThaiHoatDong == trangThaiHoatDong.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var suppliers = await query
            .OrderBy(x => x.TenNhaCungCap)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (suppliers, totalCount);
    }

    public async Task<bool> UpdateStatusAsync(int supplierId, bool trangThaiHoatDong, CancellationToken cancellationToken = default)
    {
        var supplier = await GetByIdAsync(supplierId, cancellationToken);
        if (supplier == null)
            return false;

        supplier.TrangThaiHoatDong = trangThaiHoatDong;
        Update(supplier);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<NhaCungCap>> GetTopSuppliersAsync(int take = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .OrderBy(x => x.TenNhaCungCap)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}