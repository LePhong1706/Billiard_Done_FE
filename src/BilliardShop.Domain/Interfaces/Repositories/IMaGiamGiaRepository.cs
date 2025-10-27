using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IMaGiamGiaRepository : IGenericRepository<MaGiamGia>
{
    Task<MaGiamGia?> GetByCodeAsync(string maCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaGiamGia>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MaGiamGia>> GetValidCodesAsync(CancellationToken cancellationToken = default);
    Task<bool> IsValidCodeAsync(string maCode, int? nguoiDungId = null, decimal? donHangValue = null, CancellationToken cancellationToken = default);
    Task<bool> CanUserUseCodeAsync(string maCode, int nguoiDungId, CancellationToken cancellationToken = default);
    Task<decimal> CalculateDiscountAsync(string maCode, decimal orderValue, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaGiamGia>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaGiamGia>> GetExpiringCodesAsync(int days = 7, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string maCode, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateUsageCountAsync(int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaGiamGia>> GetByDiscountTypeAsync(string loaiGiamGia, CancellationToken cancellationToken = default);
    Task<(IEnumerable<MaGiamGia> Codes, int TotalCount)> SearchCodesAsync(
        string? searchTerm = null,
        string? loaiGiamGia = null,
        bool? trangThaiHoatDong = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<MaGiamGia>> GetMostUsedCodesAsync(int top = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaGiamGia>> GetUnusedCodesAsync(CancellationToken cancellationToken = default);
    Task<bool> DeactivateExpiredCodesAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetTotalDiscountGivenAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<int> GetUsageStatsAsync(int maGiamGiaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaGiamGia>> GetAvailableForUserAsync(int nguoiDungId, decimal? orderValue = null, CancellationToken cancellationToken = default);
    Task<bool> IsCodeExpiredAsync(string maCode, CancellationToken cancellationToken = default);
    Task<bool> HasReachedUsageLimitAsync(string maCode, CancellationToken cancellationToken = default);
    Task<int> GetUserUsageCountAsync(string maCode, int nguoiDungId, CancellationToken cancellationToken = default);
}