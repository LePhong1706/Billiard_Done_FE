using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class NguoiDungRepository : GenericRepository<NguoiDung>, INguoiDungRepository
{
    public NguoiDungRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<NguoiDung?> GetByEmailAsync(string email, bool includeVaiTro = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.Email == email);

        if (includeVaiTro)
        {
            query = query.Include(x => x.VaiTro);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<NguoiDung?> GetByTenDangNhapAsync(string tenDangNhap, bool includeVaiTro = false, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TenDangNhap == tenDangNhap);

        if (includeVaiTro)
        {
            query = query.Include(x => x.VaiTro);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<NguoiDung?> GetByEmailOrUsernameAsync(string emailOrUsername, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.VaiTro)
            .FirstOrDefaultAsync(x => x.Email == emailOrUsername || x.TenDangNhap == emailOrUsername, cancellationToken);
    }

    public async Task<bool> IsEmailExistsAsync(string email, int? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.Email == email);

        if (excludeUserId.HasValue)
        {
            query = query.Where(x => x.Id != excludeUserId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> IsUsernameExistsAsync(string tenDangNhap, int? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TenDangNhap == tenDangNhap);

        if (excludeUserId.HasValue)
        {
            query = query.Where(x => x.Id != excludeUserId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> UpdateLastLoginAsync(int userId, DateTime lastLoginTime, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken);
        if (user == null)
            return false;

        user.LanDangNhapCuoi = lastLoginTime;
        Update(user);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateEmailVerificationStatusAsync(int userId, bool daXacThuc, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken);
        if (user == null)
            return false;

        user.DaXacThucEmail = daXacThuc;
        Update(user);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdatePasswordAsync(int userId, string matKhauMaHoa, string muoiMatKhau, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken);
        if (user == null)
            return false;

        user.MatKhauMaHoa = matKhauMaHoa;
        user.MuoiMatKhau = muoiMatKhau;
        Update(user);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<NguoiDung>> GetUsersByRoleAsync(int vaiTroId, bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.VaiTro)
            .Where(x => x.MaVaiTro == vaiTroId);

        if (activeOnly)
        {
            query = query.Where(x => x.TrangThaiHoatDong);
        }

        return await query
            .OrderBy(x => x.Ho)
            .ThenBy(x => x.Ten)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<NguoiDung> Users, int TotalCount)> SearchUsersAsync(
        string? searchTerm = null,
        int? vaiTroId = null,
        bool? trangThaiHoatDong = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Include(x => x.VaiTro).AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(x =>
                (x.Ho != null && x.Ho.ToLower().Contains(searchLower)) ||
                (x.Ten != null && x.Ten.ToLower().Contains(searchLower)) ||
                x.Email.ToLower().Contains(searchLower) ||
                x.TenDangNhap.ToLower().Contains(searchLower) ||
                (x.SoDienThoai != null && x.SoDienThoai.Contains(searchTerm))
            );
        }

        // Apply role filter
        if (vaiTroId.HasValue)
        {
            query = query.Where(x => x.MaVaiTro == vaiTroId.Value);
        }

        // Apply active status filter
        if (trangThaiHoatDong.HasValue)
        {
            query = query.Where(x => x.TrangThaiHoatDong == trangThaiHoatDong.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderBy(x => x.Ho)
            .ThenBy(x => x.Ten)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (users, totalCount);
    }

    public async Task<NguoiDung?> GetUserWithDetailsAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.VaiTro)
            .Include(x => x.DiaChis)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }

    public async Task<int> CountNewUsersAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .CountAsync(x => x.NgayTao >= fromDate && x.NgayTao <= toDate, cancellationToken);
    }

    public async Task<IEnumerable<NguoiDung>> GetRecentActiveUsersAsync(int days = 30, int take = 100, CancellationToken cancellationToken = default)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);

        return await _dbSet
            .Include(x => x.VaiTro)
            .Where(x => x.TrangThaiHoatDong && x.LanDangNhapCuoi >= fromDate)
            .OrderByDescending(x => x.LanDangNhapCuoi)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateActiveStatusAsync(int userId, bool trangThaiHoatDong, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken);
        if (user == null)
            return false;

        user.TrangThaiHoatDong = trangThaiHoatDong;
        Update(user);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}