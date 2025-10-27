using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IDanhGiaSanPhamRepository : IGenericRepository<DanhGiaSanPham>
{
    Task<IEnumerable<DanhGiaSanPham>> GetByProductIdAsync(int sanPhamId, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhGiaSanPham>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<DanhGiaSanPham?> GetUserReviewForProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<bool> HasUserReviewedProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<bool> CanUserReviewProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<decimal> GetAverageRatingAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> GetReviewCountAsync(int sanPhamId, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int Rating, int Count)>> GetRatingDistributionAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhGiaSanPham>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);
    Task<bool> ApproveReviewAsync(int danhGiaId, int nguoiDuyetId, CancellationToken cancellationToken = default);
    Task<bool> RejectReviewAsync(int danhGiaId, int nguoiDuyetId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<DanhGiaSanPham> Reviews, int TotalCount)> GetPagedReviewsAsync(
        int sanPhamId,
        int pageNumber = 1,
        int pageSize = 10,
        int? rating = null,
        bool approvedOnly = true,
        string sortBy = "newest",
        CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhGiaSanPham>> GetRecentReviewsAsync(int take = 10, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhGiaSanPham>> GetTopReviewsAsync(int sanPhamId, int take = 5, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhGiaSanPham>> GetReviewsByRatingAsync(int sanPhamId, int rating, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserReviewAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> CountReviewsByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhGiaSanPham>> SearchReviewsAsync(string searchTerm, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhGiaSanPham>> GetVerifiedPurchaseReviewsAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<DanhGiaSanPham> Reviews, int TotalCount)> GetPagedReviewsForModerationAsync(
        int pageNumber = 1,
        int pageSize = 20,
        bool? isDuyet = null,
        int? rating = null,
        CancellationToken cancellationToken = default);
}