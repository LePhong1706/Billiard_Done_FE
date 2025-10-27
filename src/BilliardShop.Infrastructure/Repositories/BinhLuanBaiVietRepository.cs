using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class BinhLuanBaiVietRepository : GenericRepository<BinhLuanBaiViet>, IBinhLuanBaiVietRepository
{
    public BinhLuanBaiVietRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BinhLuanBaiViet>> GetByArticleIdAsync(int baiVietId, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.BinhLuanCons.Where(r => !approvedOnly || r.TrangThai == "DaDuyet"))
                .ThenInclude(r => r.NguoiDung)
            .Where(x => x.MaBaiViet == baiVietId && x.MaBinhLuanCha == null);

        if (approvedOnly)
        {
            query = query.Where(x => x.TrangThai == "DaDuyet");
        }

        return await query
            .OrderBy(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BinhLuanBaiViet>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.BaiViet)
            .Where(x => x.MaNguoiDung == nguoiDungId)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BinhLuanBaiViet>> GetRepliesAsync(int parentCommentId, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Where(x => x.MaBinhLuanCha == parentCommentId);

        if (approvedOnly)
        {
            query = query.Where(x => x.TrangThai == "DaDuyet");
        }

        return await query
            .OrderBy(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BinhLuanBaiViet>> GetPendingCommentsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.BaiViet)
            .Include(x => x.NguoiDung)
            .Where(x => x.TrangThai == "ChoDuyet")
            .OrderBy(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BinhLuanBaiViet>> GetByStatusAsync(string trangThai, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.BaiViet)
            .Include(x => x.NguoiDung)
            .Where(x => x.TrangThai == trangThai)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ApproveCommentAsync(int binhLuanId, int nguoiDuyetId, CancellationToken cancellationToken = default)
    {
        var comment = await GetByIdAsync(binhLuanId, cancellationToken);
        if (comment == null) return false;

        comment.TrangThai = "DaDuyet";
        comment.NgayDuyet = DateTime.UtcNow;
        comment.NguoiDuyet = nguoiDuyetId;
        Update(comment);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RejectCommentAsync(int binhLuanId, int nguoiDuyetId, CancellationToken cancellationToken = default)
    {
        var comment = await GetByIdAsync(binhLuanId, cancellationToken);
        if (comment == null) return false;

        comment.TrangThai = "BiTuChoi";
        comment.NgayDuyet = DateTime.UtcNow;
        comment.NguoiDuyet = nguoiDuyetId;
        Update(comment);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateStatusAsync(int binhLuanId, string trangThai, int? nguoiDuyetId = null, CancellationToken cancellationToken = default)
    {
        var comment = await GetByIdAsync(binhLuanId, cancellationToken);
        if (comment == null) return false;

        comment.TrangThai = trangThai;
        
        if (trangThai != "ChoDuyet")
        {
            comment.NgayDuyet = DateTime.UtcNow;
            comment.NguoiDuyet = nguoiDuyetId;
        }

        Update(comment);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> CountByArticleAsync(int baiVietId, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaBaiViet == baiVietId);

        if (approvedOnly)
        {
            query = query.Where(x => x.TrangThai == "DaDuyet");
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<int> CountByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.MaNguoiDung == nguoiDungId, cancellationToken);
    }

    public async Task<int> CountByStatusAsync(string trangThai, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.TrangThai == trangThai, cancellationToken);
    }

    public async Task<(IEnumerable<BinhLuanBaiViet> Comments, int TotalCount)> GetPagedByArticleAsync(
        int baiVietId,
        int pageNumber = 1,
        int pageSize = 20,
        bool approvedOnly = true,
        string sortBy = "oldest",
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.BinhLuanCons.Where(r => !approvedOnly || r.TrangThai == "DaDuyet"))
                .ThenInclude(r => r.NguoiDung)
            .Where(x => x.MaBaiViet == baiVietId && x.MaBinhLuanCha == null);

        if (approvedOnly)
        {
            query = query.Where(x => x.TrangThai == "DaDuyet");
        }

        query = sortBy.ToLower() switch
        {
            "newest" => query.OrderByDescending(x => x.NgayTao),
            "oldest" => query.OrderBy(x => x.NgayTao),
            _ => query.OrderBy(x => x.NgayTao)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var comments = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (comments, totalCount);
    }

    public async Task<(IEnumerable<BinhLuanBaiViet> Comments, int TotalCount)> GetPagedForModerationAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? trangThai = null,
        int? baiVietId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.BaiViet)
            .Include(x => x.NguoiDung)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(trangThai))
        {
            query = query.Where(x => x.TrangThai == trangThai);
        }

        if (baiVietId.HasValue)
        {
            query = query.Where(x => x.MaBaiViet == baiVietId.Value);
        }

        query = query.OrderBy(x => x.TrangThai == "ChoDuyet" ? 0 : 1)
                    .ThenBy(x => x.NgayTao);

        var totalCount = await query.CountAsync(cancellationToken);

        var comments = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (comments, totalCount);
    }

    public async Task<IEnumerable<BinhLuanBaiViet>> GetRecentCommentsAsync(int take = 10, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.BaiViet)
            .Include(x => x.NguoiDung)
            .AsQueryable();

        if (approvedOnly)
        {
            query = query.Where(x => x.TrangThai == "DaDuyet");
        }

        return await query
            .OrderByDescending(x => x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasUserCommentedOnArticleAsync(int nguoiDungId, int baiVietId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.MaNguoiDung == nguoiDungId && x.MaBaiViet == baiVietId, cancellationToken);
    }

    public async Task<int> DeleteByArticleIdAsync(int baiVietId, CancellationToken cancellationToken = default)
    {
        var comments = await _dbSet.Where(x => x.MaBaiViet == baiVietId).ToListAsync(cancellationToken);
        
        if (!comments.Any()) return 0;

        RemoveRange(comments);
        await _context.SaveChangesAsync(cancellationToken);
        
        return comments.Count;
    }

    public async Task<int> DeleteByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        var comments = await _dbSet.Where(x => x.MaNguoiDung == nguoiDungId).ToListAsync(cancellationToken);
        
        if (!comments.Any()) return 0;

        RemoveRange(comments);
        await _context.SaveChangesAsync(cancellationToken);
        
        return comments.Count;
    }

    public async Task<IEnumerable<BinhLuanBaiViet>> SearchCommentsAsync(string searchTerm, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.BaiViet)
            .Include(x => x.NguoiDung)
            .AsQueryable();

        if (approvedOnly)
        {
            query = query.Where(x => x.TrangThai == "DaDuyet");
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(x => 
                x.NoiDungBinhLuan.ToLower().Contains(searchLower) ||
                (x.TenNguoiBinhLuan != null && x.TenNguoiBinhLuan.ToLower().Contains(searchLower)) ||
                (x.NguoiDung != null && (
                    (x.NguoiDung.Ho != null && x.NguoiDung.Ho.ToLower().Contains(searchLower)) ||
                    (x.NguoiDung.Ten != null && x.NguoiDung.Ten.ToLower().Contains(searchLower))
                ))
            );
        }

        return await query
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BinhLuanBaiViet>> GetCommentThreadAsync(int rootCommentId, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var rootComment = await _dbSet
            .Include(x => x.NguoiDung)
            .FirstOrDefaultAsync(x => x.Id == rootCommentId, cancellationToken);

        if (rootComment == null) return Enumerable.Empty<BinhLuanBaiViet>();

        var allComments = new List<BinhLuanBaiViet> { rootComment };
        await GetCommentRepliesRecursive(rootCommentId, allComments, approvedOnly, cancellationToken);

        return allComments;
    }

    public async Task<bool> IsReplyToCommentAsync(int binhLuanId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.Id == binhLuanId && x.MaBinhLuanCha != null, cancellationToken);
    }

    public async Task<IEnumerable<(DateTime Date, int Count)>> GetCommentStatsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.NgayTao >= fromDate && x.NgayTao <= toDate)
            .GroupBy(x => x.NgayTao.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken)
            .ContinueWith(task => task.Result.Select(x => (x.Date, x.Count)), cancellationToken);
    }

    private async Task GetCommentRepliesRecursive(int parentId, List<BinhLuanBaiViet> allComments, bool approvedOnly, CancellationToken cancellationToken)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Where(x => x.MaBinhLuanCha == parentId);

        if (approvedOnly)
        {
            query = query.Where(x => x.TrangThai == "DaDuyet");
        }

        var replies = await query
            .OrderBy(x => x.NgayTao)
            .ToListAsync(cancellationToken);

        foreach (var reply in replies)
        {
            allComments.Add(reply);
            await GetCommentRepliesRecursive(reply.Id, allComments, approvedOnly, cancellationToken);
        }
    }
}