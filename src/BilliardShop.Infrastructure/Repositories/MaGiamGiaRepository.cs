using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class MaGiamGiaRepository : GenericRepository<MaGiamGia>, IMaGiamGiaRepository
{
    public MaGiamGiaRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<MaGiamGia?> GetByCodeAsync(string maCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SuDungMaGiamGias)
            .FirstOrDefaultAsync(x => x.MaCode == maCode, cancellationToken);
    }

    public async Task<IEnumerable<MaGiamGia>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MaGiamGia>> GetValidCodesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong &&
                       x.NgayBatDau <= now &&
                       x.NgayKetThuc >= now &&
                       (x.SoLuotSuDungToiDa == null || x.SoLuotDaSuDung < x.SoLuotSuDungToiDa))
            .OrderBy(x => x.NgayKetThuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsValidCodeAsync(string maCode, int? nguoiDungId = null, decimal? donHangValue = null, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var code = await _dbSet
            .FirstOrDefaultAsync(x => x.MaCode == maCode, cancellationToken);

        if (code == null || !code.TrangThaiHoatDong) return false;
        if (code.NgayBatDau > now || code.NgayKetThuc < now) return false;
        if (code.SoLuotSuDungToiDa.HasValue && code.SoLuotDaSuDung >= code.SoLuotSuDungToiDa) return false;
        if (donHangValue.HasValue && donHangValue < code.GiaTriDonHangToiThieu) return false;

        if (nguoiDungId.HasValue)
        {
            var userUsageCount = await _context.SuDungMaGiamGias
                .CountAsync(x => x.MaMaGiamGia == code.Id && x.MaNguoiDung == nguoiDungId.Value, cancellationToken);
            
            if (userUsageCount >= code.SoLuotSuDungToiDaMoiNguoi) return false;
        }

        return true;
    }

    public async Task<bool> CanUserUseCodeAsync(string maCode, int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await IsValidCodeAsync(maCode, nguoiDungId, null, cancellationToken);
    }

    public async Task<decimal> CalculateDiscountAsync(string maCode, decimal orderValue, CancellationToken cancellationToken = default)
    {
        var code = await GetByCodeAsync(maCode, cancellationToken);
        if (code == null || !await IsValidCodeAsync(maCode, null, orderValue, cancellationToken))
            return 0;

        decimal discount = code.LoaiGiamGia switch
        {
            "PhanTram" => orderValue * (code.GiaTriGiamGia / 100),
            "SoTienCoDinh" => code.GiaTriGiamGia,
            _ => 0
        };

        if (code.SoTienGiamToiDa.HasValue && discount > code.SoTienGiamToiDa.Value)
            discount = code.SoTienGiamToiDa.Value;

        return discount;
    }

    public async Task<IEnumerable<MaGiamGia>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.NgayTao >= fromDate && x.NgayTao <= toDate)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MaGiamGia>> GetExpiringCodesAsync(int days = 7, CancellationToken cancellationToken = default)
    {
        var futureDate = DateTime.UtcNow.AddDays(days);
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong &&
                       x.NgayKetThuc <= futureDate &&
                       x.NgayKetThuc >= DateTime.UtcNow)
            .OrderBy(x => x.NgayKetThuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string maCode, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaCode == maCode);
        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> UpdateUsageCountAsync(int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        var code = await GetByIdAsync(maGiamGiaId, cancellationToken);
        if (code == null) return false;

        code.SoLuotDaSuDung++;
        Update(code);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<MaGiamGia>> GetByDiscountTypeAsync(string loaiGiamGia, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.LoaiGiamGia == loaiGiamGia && x.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<MaGiamGia> Codes, int TotalCount)> SearchCodesAsync(
        string? searchTerm = null,
        string? loaiGiamGia = null,
        bool? trangThaiHoatDong = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(x => x.MaCode.ToLower().Contains(searchLower) ||
                                   x.TenMaGiamGia.ToLower().Contains(searchLower) ||
                                   (x.MoTa != null && x.MoTa.ToLower().Contains(searchLower)));
        }

        if (!string.IsNullOrEmpty(loaiGiamGia))
            query = query.Where(x => x.LoaiGiamGia == loaiGiamGia);

        if (trangThaiHoatDong.HasValue)
            query = query.Where(x => x.TrangThaiHoatDong == trangThaiHoatDong.Value);

        if (fromDate.HasValue)
            query = query.Where(x => x.NgayTao >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.NgayTao <= toDate.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var codes = await query
            .OrderByDescending(x => x.NgayTao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (codes, totalCount);
    }

    public async Task<IEnumerable<MaGiamGia>> GetMostUsedCodesAsync(int top = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .OrderByDescending(x => x.SoLuotDaSuDung)
            .Take(top)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MaGiamGia>> GetUnusedCodesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.SoLuotDaSuDung == 0 && x.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeactivateExpiredCodesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var expiredCodes = await _dbSet
            .Where(x => x.TrangThaiHoatDong && x.NgayKetThuc < now)
            .ToListAsync(cancellationToken);

        if (!expiredCodes.Any()) return true;

        foreach (var code in expiredCodes)
        {
            code.TrangThaiHoatDong = false;
            Update(code);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<decimal> GetTotalDiscountGivenAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.SuDungMaGiamGias.AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(x => x.NgaySuDung >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.NgaySuDung <= toDate.Value);

        return await query.SumAsync(x => x.SoTienGiamGia, cancellationToken);
    }

    public async Task<int> GetUsageStatsAsync(int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        return await _context.SuDungMaGiamGias
            .CountAsync(x => x.MaMaGiamGia == maGiamGiaId, cancellationToken);
    }

    public async Task<IEnumerable<MaGiamGia>> GetAvailableForUserAsync(int nguoiDungId, decimal? orderValue = null, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var query = _dbSet
            .Where(x => x.TrangThaiHoatDong &&
                       x.NgayBatDau <= now &&
                       x.NgayKetThuc >= now &&
                       (x.SoLuotSuDungToiDa == null || x.SoLuotDaSuDung < x.SoLuotSuDungToiDa));

        if (orderValue.HasValue)
            query = query.Where(x => orderValue >= x.GiaTriDonHangToiThieu);

        var availableCodes = await query.ToListAsync(cancellationToken);
        var result = new List<MaGiamGia>();

        foreach (var code in availableCodes)
        {
            var userUsageCount = await _context.SuDungMaGiamGias
                .CountAsync(x => x.MaMaGiamGia == code.Id && x.MaNguoiDung == nguoiDungId, cancellationToken);

            if (userUsageCount < code.SoLuotSuDungToiDaMoiNguoi)
                result.Add(code);
        }

        return result.OrderBy(x => x.NgayKetThuc);
    }

    public async Task<bool> IsCodeExpiredAsync(string maCode, CancellationToken cancellationToken = default)
    {
        var code = await _dbSet
            .Where(x => x.MaCode == maCode)
            .Select(x => x.NgayKetThuc)
            .FirstOrDefaultAsync(cancellationToken);

        return code != default && code < DateTime.UtcNow;
    }

    public async Task<bool> HasReachedUsageLimitAsync(string maCode, CancellationToken cancellationToken = default)
    {
        var code = await _dbSet
            .Where(x => x.MaCode == maCode)
            .Select(x => new { x.SoLuotSuDungToiDa, x.SoLuotDaSuDung })
            .FirstOrDefaultAsync(cancellationToken);

        return code != null && code.SoLuotSuDungToiDa.HasValue && code.SoLuotDaSuDung >= code.SoLuotSuDungToiDa.Value;
    }

    public async Task<int> GetUserUsageCountAsync(string maCode, int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _context.SuDungMaGiamGias
            .Include(x => x.MaGiamGia)
            .CountAsync(x => x.MaGiamGia.MaCode == maCode && x.MaNguoiDung == nguoiDungId, cancellationToken);
    }
}