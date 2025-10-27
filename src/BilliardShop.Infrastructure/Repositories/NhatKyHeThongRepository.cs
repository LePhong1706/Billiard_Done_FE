using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class NhatKyHeThongRepository : GenericRepository<NhatKyHeThong>, INhatKyHeThongRepository
{
    public NhatKyHeThongRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<NhatKyHeThong> LogAsync(
        string tenBang,
        int maBanGhi,
        string hanhDong,
        string? giaTriCu = null,
        string? giaTriMoi = null,
        int? maNguoiDung = null,
        string? diaChiIP = null,
        string? thongTinTrinhDuyet = null,
        CancellationToken cancellationToken = default)
    {
        var log = new NhatKyHeThong
        {
            TenBang = tenBang,
            MaBanGhi = maBanGhi,
            HanhDong = hanhDong,
            GiaTriCu = giaTriCu,
            GiaTriMoi = giaTriMoi,
            MaNguoiDung = maNguoiDung,
            DiaChiIP = diaChiIP,
            ThongTinTrinhDuyet = thongTinTrinhDuyet,
            ThoiGian = DateTime.UtcNow
        };

        var result = await AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result;
    }

    public async Task<NhatKyHeThong> LogCreateAsync(
        string tenBang,
        int maBanGhi,
        string? giaTriMoi = null,
        int? maNguoiDung = null,
        string? diaChiIP = null,
        string? thongTinTrinhDuyet = null,
        CancellationToken cancellationToken = default)
    {
        return await LogAsync(tenBang, maBanGhi, "THEM", null, giaTriMoi, maNguoiDung, diaChiIP, thongTinTrinhDuyet, cancellationToken);
    }

    public async Task<NhatKyHeThong> LogUpdateAsync(
        string tenBang,
        int maBanGhi,
        string? giaTriCu = null,
        string? giaTriMoi = null,
        int? maNguoiDung = null,
        string? diaChiIP = null,
        string? thongTinTrinhDuyet = null,
        CancellationToken cancellationToken = default)
    {
        return await LogAsync(tenBang, maBanGhi, "SUA", giaTriCu, giaTriMoi, maNguoiDung, diaChiIP, thongTinTrinhDuyet, cancellationToken);
    }

    public async Task<NhatKyHeThong> LogDeleteAsync(
        string tenBang,
        int maBanGhi,
        string? giaTriCu = null,
        int? maNguoiDung = null,
        string? diaChiIP = null,
        string? thongTinTrinhDuyet = null,
        CancellationToken cancellationToken = default)
    {
        return await LogAsync(tenBang, maBanGhi, "XOA", giaTriCu, null, maNguoiDung, diaChiIP, thongTinTrinhDuyet, cancellationToken);
    }

    public async Task<IEnumerable<NhatKyHeThong>> GetByTableAndRecordAsync(
        string tenBang,
        int maBanGhi,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Where(x => x.TenBang == tenBang && x.MaBanGhi == maBanGhi)
            .OrderByDescending(x => x.ThoiGian)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<NhatKyHeThong>> GetByUserAsync(
        int maNguoiDung,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Where(x => x.MaNguoiDung == maNguoiDung);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian <= toDate.Value);
        }

        return await query
            .OrderByDescending(x => x.ThoiGian)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<NhatKyHeThong>> GetByTableAsync(
        string tenBang,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Where(x => x.TenBang == tenBang);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian <= toDate.Value);
        }

        return await query
            .OrderByDescending(x => x.ThoiGian)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<NhatKyHeThong>> GetByActionAsync(
        string hanhDong,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Where(x => x.HanhDong == hanhDong);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian <= toDate.Value);
        }

        return await query
            .OrderByDescending(x => x.ThoiGian)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<NhatKyHeThong> Logs, int TotalCount)> GetPagedLogsAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? tenBang = null,
        string? hanhDong = null,
        int? maNguoiDung = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Include(x => x.NguoiDung).AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(tenBang))
        {
            query = query.Where(x => x.TenBang == tenBang);
        }

        if (!string.IsNullOrEmpty(hanhDong))
        {
            query = query.Where(x => x.HanhDong == hanhDong);
        }

        if (maNguoiDung.HasValue)
        {
            query = query.Where(x => x.MaNguoiDung == maNguoiDung.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian <= toDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var logs = await query
            .OrderByDescending(x => x.ThoiGian)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (logs, totalCount);
    }

    public async Task<int> CountByTableAsync(
        string tenBang,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TenBang == tenBang);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian <= toDate.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<int> CountByUserAsync(
        int maNguoiDung,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaNguoiDung == maNguoiDung);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian <= toDate.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<(DateTime Date, int Count)>> GetDailyActivityStatsAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.ThoiGian >= fromDate && x.ThoiGian <= toDate)
            .GroupBy(x => x.ThoiGian.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken)
            .ContinueWith(task => task.Result.Select(x => (x.Date, x.Count)), cancellationToken);
    }

    public async Task<IEnumerable<(string TableName, int Count)>> GetTableActivityStatsAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian <= toDate.Value);
        }

        return await query
            .GroupBy(x => x.TenBang)
            .Select(g => new { TableName = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync(cancellationToken)
            .ContinueWith(task => task.Result.Select(x => (x.TableName, x.Count)), cancellationToken);
    }

    public async Task<IEnumerable<(int UserId, int Count)>> GetUserActivityStatsAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int top = 10,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaNguoiDung != null);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian <= toDate.Value);
        }

        return await query
            .GroupBy(x => x.MaNguoiDung!.Value)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(top)
            .ToListAsync(cancellationToken)
            .ContinueWith(task => task.Result.Select(x => (x.UserId, x.Count)), cancellationToken);
    }

    public async Task<int> CleanupOldLogsAsync(int daysToKeep = 90, CancellationToken cancellationToken = default)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);
        
        var oldLogs = await _dbSet
            .Where(x => x.ThoiGian < cutoffDate)
            .ToListAsync(cancellationToken);

        if (!oldLogs.Any())
            return 0;

        RemoveRange(oldLogs);
        await _context.SaveChangesAsync(cancellationToken);
        
        return oldLogs.Count;
    }

    public async Task<IEnumerable<NhatKyHeThong>> SearchLogsAsync(
        string searchTerm,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var searchLower = searchTerm.ToLower();
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Where(x => x.TenBang.ToLower().Contains(searchLower) ||
                       x.HanhDong.ToLower().Contains(searchLower) ||
                       (x.GiaTriCu != null && x.GiaTriCu.ToLower().Contains(searchLower)) ||
                       (x.GiaTriMoi != null && x.GiaTriMoi.ToLower().Contains(searchLower)) ||
                       (x.DiaChiIP != null && x.DiaChiIP.Contains(searchTerm)));

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ThoiGian <= toDate.Value);
        }

        return await query
            .OrderByDescending(x => x.ThoiGian)
            .ToListAsync(cancellationToken);
    }
}