using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class DonHangRepository : GenericRepository<DonHang>, IDonHangRepository
{
    public DonHangRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<DonHang?> GetByOrderNumberAsync(string soDonHang, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .Include(x => x.PhuongThucThanhToan)
            .Include(x => x.PhuongThucVanChuyen)
            .FirstOrDefaultAsync(x => x.SoDonHang == soDonHang, cancellationToken);
    }

    public async Task<DonHang?> GetOrderWithDetailsAsync(int donHangId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .Include(x => x.PhuongThucThanhToan)
            .Include(x => x.PhuongThucVanChuyen)
            .Include(x => x.ChiTietDonHangs).ThenInclude(ct => ct.SanPham)
            .Include(x => x.SuDungMaGiamGias).ThenInclude(mg => mg.MaGiamGia)
            .FirstOrDefaultAsync(x => x.Id == donHangId, cancellationToken);
    }

    public async Task<IEnumerable<DonHang>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TrangThai)
            .Include(x => x.PhuongThucThanhToan)
            .Include(x => x.PhuongThucVanChuyen)
            .Include(x => x.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
            .Where(x => x.MaNguoiDung == nguoiDungId)
            .OrderByDescending(x => x.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DonHang>> GetByStatusAsync(int trangThaiId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .Where(x => x.MaTrangThai == trangThaiId)
            .OrderByDescending(x => x.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DonHang>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .Where(x => x.NgayDatHang >= fromDate && x.NgayDatHang <= toDate)
            .OrderByDescending(x => x.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DonHang>> GetPendingOrdersAsync(CancellationToken cancellationToken = default)
    {
        var pendingStatusNames = new[] { "ChoDuyet", "DangXuLy", "DongGoi" };
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .Where(x => pendingStatusNames.Contains(x.TrangThai.TenTrangThai))
            .OrderBy(x => x.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DonHang>> GetRecentOrdersAsync(int days = 7, CancellationToken cancellationToken = default)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .Where(x => x.NgayDatHang >= fromDate)
            .OrderByDescending(x => x.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateStatusAsync(int donHangId, int newStatusId, CancellationToken cancellationToken = default)
    {
        var order = await GetByIdAsync(donHangId, cancellationToken);
        if (order == null) return false;

        order.MaTrangThai = newStatusId;
        Update(order);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdatePaymentStatusAsync(int donHangId, string paymentStatus, CancellationToken cancellationToken = default)
    {
        var order = await GetByIdAsync(donHangId, cancellationToken);
        if (order == null) return false;

        order.TrangThaiThanhToan = paymentStatus;
        Update(order);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TrangThaiThanhToan == "DaThanhToan");

        if (fromDate.HasValue)
            query = query.Where(x => x.NgayDatHang >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.NgayDatHang <= toDate.Value);

        return await query.SumAsync(x => x.TongThanhToan, cancellationToken);
    }

    public async Task<int> CountOrdersByStatusAsync(int trangThaiId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.MaTrangThai == trangThaiId, cancellationToken);
    }

    public async Task<(IEnumerable<DonHang> Orders, int TotalCount)> SearchOrdersAsync(
        string? searchTerm = null,
        int? nguoiDungId = null,
        int? trangThaiId = null,
        string? trangThaiThanhToan = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        decimal? minAmount = null,
        decimal? maxAmount = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .Include(x => x.PhuongThucThanhToan)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(x =>
                x.SoDonHang.ToLower().Contains(searchLower) ||
                (x.TenKhachHang != null && x.TenKhachHang.ToLower().Contains(searchLower)) ||
                (x.EmailKhachHang != null && x.EmailKhachHang.ToLower().Contains(searchLower)) ||
                (x.NguoiDung != null && (x.NguoiDung.Ho + " " + x.NguoiDung.Ten).ToLower().Contains(searchLower))
            );
        }

        if (nguoiDungId.HasValue)
            query = query.Where(x => x.MaNguoiDung == nguoiDungId.Value);

        if (trangThaiId.HasValue)
            query = query.Where(x => x.MaTrangThai == trangThaiId.Value);

        if (!string.IsNullOrEmpty(trangThaiThanhToan))
            query = query.Where(x => x.TrangThaiThanhToan == trangThaiThanhToan);

        if (fromDate.HasValue)
            query = query.Where(x => x.NgayDatHang >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.NgayDatHang <= toDate.Value);

        if (minAmount.HasValue)
            query = query.Where(x => x.TongThanhToan >= minAmount.Value);

        if (maxAmount.HasValue)
            query = query.Where(x => x.TongThanhToan <= maxAmount.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(x => x.NgayDatHang)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (orders, totalCount);
    }

    public async Task<IEnumerable<DonHang>> GetOrdersByPaymentMethodAsync(int phuongThucThanhToanId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .Where(x => x.MaPhuongThucThanhToan == phuongThucThanhToanId)
            .OrderByDescending(x => x.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DonHang>> GetOrdersByShippingMethodAsync(int phuongThucVanChuyenId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .Where(x => x.MaPhuongThucVanChuyen == phuongThucVanChuyenId)
            .OrderByDescending(x => x.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GenerateOrderNumberAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var count = await _dbSet
            .CountAsync(x => x.SoDonHang.StartsWith("DH" + today), cancellationToken);
        
        return $"DH{today}{(count + 1):D4}";
    }

    public async Task<bool> ExistsByOrderNumberAsync(string soDonHang, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.SoDonHang == soDonHang, cancellationToken);
    }

    public async Task<IEnumerable<(DateTime Date, decimal Revenue)>> GetDailyRevenueStatsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var results = await _dbSet
            .Where(x => x.TrangThaiThanhToan == "DaThanhToan" && 
                       x.NgayDatHang >= fromDate && 
                       x.NgayDatHang <= toDate)
            .GroupBy(x => x.NgayDatHang.Date)
            .Select(g => new { Date = g.Key, Revenue = g.Sum(x => x.TongThanhToan) })
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.Date, x.Revenue));
    }

    public async Task<IEnumerable<(int Month, int Year, decimal Revenue)>> GetMonthlyRevenueStatsAsync(int year, CancellationToken cancellationToken = default)
    {
        var results = await _dbSet
            .Where(x => x.TrangThaiThanhToan == "DaThanhToan" && x.NgayDatHang.Year == year)
            .GroupBy(x => new { x.NgayDatHang.Month, x.NgayDatHang.Year })
            .Select(g => new { g.Key.Month, g.Key.Year, Revenue = g.Sum(x => x.TongThanhToan) })
            .OrderBy(x => x.Month)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.Month, x.Year, x.Revenue));
    }

    public async Task<IEnumerable<DonHang>> GetTopOrdersByAmountAsync(int top = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.TrangThai)
            .OrderByDescending(x => x.TongThanhToan)
            .Take(top)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetAverageOrderValueAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(x => x.NgayDatHang >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.NgayDatHang <= toDate.Value);

        var orders = await query.Select(x => x.TongThanhToan).ToListAsync(cancellationToken);
        return orders.Any() ? orders.Average() : 0;
    }

    public async Task<bool> CanCancelOrderAsync(int donHangId, CancellationToken cancellationToken = default)
    {
        var order = await _dbSet
            .Include(x => x.TrangThai)
            .FirstOrDefaultAsync(x => x.Id == donHangId, cancellationToken);

        if (order == null) return false;

        var cancelableStatuses = new[] { "ChoDuyet", "DangXuLy" };
        return cancelableStatuses.Contains(order.TrangThai.TenTrangThai);
    }

    public async Task<bool> CancelOrderAsync(int donHangId, string reason, CancellationToken cancellationToken = default)
    {
        var order = await GetByIdAsync(donHangId, cancellationToken);
        if (order == null || !await CanCancelOrderAsync(donHangId, cancellationToken))
            return false;

        var canceledStatus = await _context.TrangThaiDonHangs
            .FirstOrDefaultAsync(x => x.TenTrangThai == "DaHuy", cancellationToken);

        if (canceledStatus == null) return false;

        order.MaTrangThai = canceledStatus.Id;
        order.GhiChuQuanTri = reason;
        Update(order);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}