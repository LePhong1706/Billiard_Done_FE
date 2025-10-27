using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class SuDungMaGiamGiaRepository : GenericRepository<SuDungMaGiamGia>, ISuDungMaGiamGiaRepository
{
    public SuDungMaGiamGiaRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SuDungMaGiamGia>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.MaGiamGia)
            .Include(x => x.DonHang)
            .Where(x => x.MaNguoiDung == nguoiDungId)
            .OrderByDescending(x => x.NgaySuDung)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SuDungMaGiamGia>> GetByDiscountCodeIdAsync(int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.DonHang)
            .Where(x => x.MaMaGiamGia == maGiamGiaId)
            .OrderByDescending(x => x.NgaySuDung)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SuDungMaGiamGia>> GetByOrderIdAsync(int donHangId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.MaGiamGia)
            .Include(x => x.NguoiDung)
            .Where(x => x.MaDonHang == donHangId)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByDiscountCodeAsync(int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.MaMaGiamGia == maGiamGiaId, cancellationToken);
    }

    public async Task<int> CountByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.MaNguoiDung == nguoiDungId, cancellationToken);
    }

    public async Task<int> CountByUserAndCodeAsync(int nguoiDungId, int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.MaNguoiDung == nguoiDungId && x.MaMaGiamGia == maGiamGiaId, cancellationToken);
    }

    public async Task<decimal> GetTotalDiscountByCodeAsync(int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaMaGiamGia == maGiamGiaId)
            .SumAsync(x => x.SoTienGiamGia, cancellationToken);
    }

    public async Task<decimal> GetTotalDiscountByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaNguoiDung == nguoiDungId)
            .SumAsync(x => x.SoTienGiamGia, cancellationToken);
    }

    public async Task<IEnumerable<SuDungMaGiamGia>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.MaGiamGia)
            .Include(x => x.NguoiDung)
            .Include(x => x.DonHang)
            .Where(x => x.NgaySuDung >= fromDate && x.NgaySuDung <= toDate)
            .OrderByDescending(x => x.NgaySuDung)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasUserUsedCodeAsync(int nguoiDungId, int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.MaNguoiDung == nguoiDungId && x.MaMaGiamGia == maGiamGiaId, cancellationToken);
    }

    public async Task<bool> HasUserUsedCodeAsync(int nguoiDungId, string maCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.MaGiamGia)
            .AnyAsync(x => x.MaNguoiDung == nguoiDungId && x.MaGiamGia.MaCode == maCode, cancellationToken);
    }

    public async Task<SuDungMaGiamGia?> GetLatestUsageByUserAsync(int nguoiDungId, int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.MaGiamGia)
            .Include(x => x.DonHang)
            .Where(x => x.MaNguoiDung == nguoiDungId && x.MaMaGiamGia == maGiamGiaId)
            .OrderByDescending(x => x.NgaySuDung)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<SuDungMaGiamGia>> GetRecentUsagesAsync(int take = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.MaGiamGia)
            .Include(x => x.NguoiDung)
            .Include(x => x.DonHang)
            .OrderByDescending(x => x.NgaySuDung)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<SuDungMaGiamGia> Usages, int TotalCount)> GetPagedUsagesAsync(
        int pageNumber = 1,
        int pageSize = 20,
        int? nguoiDungId = null,
        int? maGiamGiaId = null,
        int? donHangId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.MaGiamGia)
            .Include(x => x.NguoiDung)
            .Include(x => x.DonHang)
            .AsQueryable();

        if (nguoiDungId.HasValue)
            query = query.Where(x => x.MaNguoiDung == nguoiDungId.Value);

        if (maGiamGiaId.HasValue)
            query = query.Where(x => x.MaMaGiamGia == maGiamGiaId.Value);

        if (donHangId.HasValue)
            query = query.Where(x => x.MaDonHang == donHangId.Value);

        if (fromDate.HasValue)
            query = query.Where(x => x.NgaySuDung >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.NgaySuDung <= toDate.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var usages = await query
            .OrderByDescending(x => x.NgaySuDung)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (usages, totalCount);
    }

    public async Task<IEnumerable<(int UserId, int UsageCount, decimal TotalDiscount)>> GetTopUsersAsync(int top = 10, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(x => x.NgaySuDung >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.NgaySuDung <= toDate.Value);

        var results = await query
            .Where(x => x.MaNguoiDung != null)
            .GroupBy(x => x.MaNguoiDung!.Value)
            .Select(g => new
            {
                UserId = g.Key,
                UsageCount = g.Count(),
                TotalDiscount = g.Sum(x => x.SoTienGiamGia)
            })
            .OrderByDescending(x => x.TotalDiscount)
            .Take(top)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.UserId, x.UsageCount, x.TotalDiscount));
    }

    public async Task<IEnumerable<(int CodeId, string CodeName, int UsageCount, decimal TotalDiscount)>> GetMostUsedCodesAsync(int top = 10, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Include(x => x.MaGiamGia).AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(x => x.NgaySuDung >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.NgaySuDung <= toDate.Value);

        var results = await query
            .GroupBy(x => new { x.MaMaGiamGia, x.MaGiamGia.TenMaGiamGia })
            .Select(g => new
            {
                CodeId = g.Key.MaMaGiamGia,
                CodeName = g.Key.TenMaGiamGia,
                UsageCount = g.Count(),
                TotalDiscount = g.Sum(x => x.SoTienGiamGia)
            })
            .OrderByDescending(x => x.UsageCount)
            .Take(top)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.CodeId, x.CodeName, x.UsageCount, x.TotalDiscount));
    }

    public async Task<IEnumerable<(DateTime Date, int UsageCount, decimal TotalDiscount)>> GetDailyUsageStatsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var results = await _dbSet
            .Where(x => x.NgaySuDung >= fromDate && x.NgaySuDung <= toDate)
            .GroupBy(x => x.NgaySuDung.Date)
            .Select(g => new
            {
                Date = g.Key,
                UsageCount = g.Count(),
                TotalDiscount = g.Sum(x => x.SoTienGiamGia)
            })
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.Date, x.UsageCount, x.TotalDiscount));
    }

    public async Task<IEnumerable<(int Month, int Year, int UsageCount, decimal TotalDiscount)>> GetMonthlyUsageStatsAsync(int year, CancellationToken cancellationToken = default)
    {
        var results = await _dbSet
            .Where(x => x.NgaySuDung.Year == year)
            .GroupBy(x => new { x.NgaySuDung.Month, x.NgaySuDung.Year })
            .Select(g => new
            {
                g.Key.Month,
                g.Key.Year,
                UsageCount = g.Count(),
                TotalDiscount = g.Sum(x => x.SoTienGiamGia)
            })
            .OrderBy(x => x.Month)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.Month, x.Year, x.UsageCount, x.TotalDiscount));
    }

    public async Task<decimal> GetAverageDiscountAmountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(x => x.NgaySuDung >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.NgaySuDung <= toDate.Value);

        var discounts = await query.Select(x => x.SoTienGiamGia).ToListAsync(cancellationToken);
        return discounts.Any() ? discounts.Average() : 0;
    }

    public async Task<SuDungMaGiamGia?> GetByOrderAndCodeAsync(int donHangId, int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.MaGiamGia)
            .Include(x => x.NguoiDung)
            .FirstOrDefaultAsync(x => x.MaDonHang == donHangId && x.MaMaGiamGia == maGiamGiaId, cancellationToken);
    }

    public async Task<bool> RemoveUsageAsync(int donHangId, int maGiamGiaId, CancellationToken cancellationToken = default)
    {
        var usage = await GetByOrderAndCodeAsync(donHangId, maGiamGiaId, cancellationToken);
        if (usage == null) return false;

        Remove(usage);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<SuDungMaGiamGia>> GetUsageHistoryAsync(int nguoiDungId, int? maGiamGiaId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.MaGiamGia)
            .Include(x => x.DonHang)
            .Where(x => x.MaNguoiDung == nguoiDungId);

        if (maGiamGiaId.HasValue)
            query = query.Where(x => x.MaMaGiamGia == maGiamGiaId.Value);

        return await query
            .OrderByDescending(x => x.NgaySuDung)
            .ToListAsync(cancellationToken);
    }
}