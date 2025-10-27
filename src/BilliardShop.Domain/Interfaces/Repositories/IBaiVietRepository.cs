using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IBaiVietRepository : IGenericRepository<BaiViet>
{
    Task<BaiViet?> GetBySlugAsync(string duongDanBaiViet, CancellationToken cancellationToken = default);
    Task<BaiViet?> GetPublishedBySlugAsync(string duongDanBaiViet, CancellationToken cancellationToken = default);
    Task<BaiViet?> GetWithDetailsAsync(int baiVietId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetPublishedAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetFeaturedAsync(int take = 5, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetRecentAsync(int take = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetByAuthorAsync(int tacGiaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetByStatusAsync(string trangThai, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetDraftsByAuthorAsync(int tacGiaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetPendingPublishAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsBySlugAsync(string duongDanBaiViet, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> PublishAsync(int baiVietId, CancellationToken cancellationToken = default);
    Task<bool> UnpublishAsync(int baiVietId, CancellationToken cancellationToken = default);
    Task<bool> SetFeaturedAsync(int baiVietId, bool featured, CancellationToken cancellationToken = default);
    Task<bool> IncrementViewCountAsync(int baiVietId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<BaiViet> Articles, int TotalCount)> SearchAsync(
        string? searchTerm = null,
        int? tacGiaId = null,
        string? trangThai = null,
        bool? noiBat = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetMostViewedAsync(int days = 30, int take = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetRelatedAsync(int baiVietId, int take = 5, CancellationToken cancellationToken = default);
    Task<int> CountByStatusAsync(string trangThai, CancellationToken cancellationToken = default);
    Task<int> CountByAuthorAsync(int tacGiaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<(DateTime Date, int Count)>> GetPublishStatsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<bool> UpdateStatusAsync(int baiVietId, string trangThai, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetAllTagsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BaiViet>> GetScheduledForPublishAsync(CancellationToken cancellationToken = default);
}