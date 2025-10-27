using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IBinhLuanBaiVietRepository : IGenericRepository<BinhLuanBaiViet>
{
    Task<IEnumerable<BinhLuanBaiViet>> GetByArticleIdAsync(int baiVietId, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<BinhLuanBaiViet>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BinhLuanBaiViet>> GetRepliesAsync(int parentCommentId, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<BinhLuanBaiViet>> GetPendingCommentsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BinhLuanBaiViet>> GetByStatusAsync(string trangThai, CancellationToken cancellationToken = default);
    Task<bool> ApproveCommentAsync(int binhLuanId, int nguoiDuyetId, CancellationToken cancellationToken = default);
    Task<bool> RejectCommentAsync(int binhLuanId, int nguoiDuyetId, CancellationToken cancellationToken = default);
    Task<bool> UpdateStatusAsync(int binhLuanId, string trangThai, int? nguoiDuyetId = null, CancellationToken cancellationToken = default);
    Task<int> CountByArticleAsync(int baiVietId, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<int> CountByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<int> CountByStatusAsync(string trangThai, CancellationToken cancellationToken = default);
    Task<(IEnumerable<BinhLuanBaiViet> Comments, int TotalCount)> GetPagedByArticleAsync(
        int baiVietId,
        int pageNumber = 1,
        int pageSize = 20,
        bool approvedOnly = true,
        string sortBy = "oldest",
        CancellationToken cancellationToken = default);
    Task<(IEnumerable<BinhLuanBaiViet> Comments, int TotalCount)> GetPagedForModerationAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? trangThai = null,
        int? baiVietId = null,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<BinhLuanBaiViet>> GetRecentCommentsAsync(int take = 10, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<bool> HasUserCommentedOnArticleAsync(int nguoiDungId, int baiVietId, CancellationToken cancellationToken = default);
    Task<int> DeleteByArticleIdAsync(int baiVietId, CancellationToken cancellationToken = default);
    Task<int> DeleteByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BinhLuanBaiViet>> SearchCommentsAsync(string searchTerm, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<BinhLuanBaiViet>> GetCommentThreadAsync(int rootCommentId, bool approvedOnly = true, CancellationToken cancellationToken = default);
    Task<bool> IsReplyToCommentAsync(int binhLuanId, CancellationToken cancellationToken = default);
    Task<IEnumerable<(DateTime Date, int Count)>> GetCommentStatsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
}