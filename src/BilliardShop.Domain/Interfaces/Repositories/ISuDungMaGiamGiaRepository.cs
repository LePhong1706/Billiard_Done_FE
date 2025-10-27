using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface ISuDungMaGiamGiaRepository : IGenericRepository<SuDungMaGiamGia>
{
    Task<IEnumerable<SuDungMaGiamGia>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SuDungMaGiamGia>> GetByDiscountCodeIdAsync(int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SuDungMaGiamGia>> GetByOrderIdAsync(int donHangId, CancellationToken cancellationToken = default);
    Task<int> CountByDiscountCodeAsync(int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<int> CountByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<int> CountByUserAndCodeAsync(int nguoiDungId, int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalDiscountByCodeAsync(int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalDiscountByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SuDungMaGiamGia>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<bool> HasUserUsedCodeAsync(int nguoiDungId, int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<bool> HasUserUsedCodeAsync(int nguoiDungId, string maCode, CancellationToken cancellationToken = default);
    Task<SuDungMaGiamGia?> GetLatestUsageByUserAsync(int nguoiDungId, int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SuDungMaGiamGia>> GetRecentUsagesAsync(int take = 10, CancellationToken cancellationToken = default);
    Task<(IEnumerable<SuDungMaGiamGia> Usages, int TotalCount)> GetPagedUsagesAsync(
        int pageNumber = 1,
        int pageSize = 20,
        int? nguoiDungId = null,
        int? maGiamGiaId = null,
        int? donHangId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<(int UserId, int UsageCount, decimal TotalDiscount)>> GetTopUsersAsync(int top = 10, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int CodeId, string CodeName, int UsageCount, decimal TotalDiscount)>> GetMostUsedCodesAsync(int top = 10, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<(DateTime Date, int UsageCount, decimal TotalDiscount)>> GetDailyUsageStatsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int Month, int Year, int UsageCount, decimal TotalDiscount)>> GetMonthlyUsageStatsAsync(int year, CancellationToken cancellationToken = default);
    Task<decimal> GetAverageDiscountAmountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<SuDungMaGiamGia?> GetByOrderAndCodeAsync(int donHangId, int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<bool> RemoveUsageAsync(int donHangId, int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SuDungMaGiamGia>> GetUsageHistoryAsync(int nguoiDungId, int? maGiamGiaId = null, CancellationToken cancellationToken = default);
}