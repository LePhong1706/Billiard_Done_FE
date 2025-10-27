using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IDonHangRepository : IGenericRepository<DonHang>
{
    Task<DonHang?> GetByOrderNumberAsync(string soDonHang, CancellationToken cancellationToken = default);
    Task<DonHang?> GetOrderWithDetailsAsync(int donHangId, CancellationToken cancellationToken = default);

    Task<IEnumerable<DonHang>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DonHang>> GetByStatusAsync(int trangThaiId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DonHang>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<DonHang>> GetPendingOrdersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DonHang>> GetRecentOrdersAsync(int days = 7, CancellationToken cancellationToken = default);
    Task<bool> UpdateStatusAsync(int donHangId, int newStatusId, CancellationToken cancellationToken = default);
    Task<bool> UpdatePaymentStatusAsync(int donHangId, string paymentStatus, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<int> CountOrdersByStatusAsync(int trangThaiId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<DonHang> Orders, int TotalCount)> SearchOrdersAsync(
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
        CancellationToken cancellationToken = default);
    Task<IEnumerable<DonHang>> GetOrdersByPaymentMethodAsync(int phuongThucThanhToanId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DonHang>> GetOrdersByShippingMethodAsync(int phuongThucVanChuyenId, CancellationToken cancellationToken = default);
    Task<string> GenerateOrderNumberAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByOrderNumberAsync(string soDonHang, CancellationToken cancellationToken = default);
    Task<IEnumerable<(DateTime Date, decimal Revenue)>> GetDailyRevenueStatsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int Month, int Year, decimal Revenue)>> GetMonthlyRevenueStatsAsync(int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<DonHang>> GetTopOrdersByAmountAsync(int top = 10, CancellationToken cancellationToken = default);
    Task<decimal> GetAverageOrderValueAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<bool> CanCancelOrderAsync(int donHangId, CancellationToken cancellationToken = default);
    Task<bool> CancelOrderAsync(int donHangId, string reason, CancellationToken cancellationToken = default);
}