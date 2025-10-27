using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class DanhGiaSanPhamRepository : GenericRepository<DanhGiaSanPham>, IDanhGiaSanPhamRepository
{
    public DanhGiaSanPhamRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DanhGiaSanPham>> GetByProductIdAsync(int sanPhamId, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.DonHang)
            .Where(x => x.MaSanPham == sanPhamId);

        if (approvedOnly)
        {
            query = query.Where(x => x.DaDuyet);
        }

        return await query
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhGiaSanPham>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.NguoiDuyetNavigation)
            .Where(x => x.MaNguoiDung == nguoiDungId)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<DanhGiaSanPham?> GetUserReviewForProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .Include(x => x.DonHang)
            .Include(x => x.NguoiDuyetNavigation)
            .FirstOrDefaultAsync(x => x.MaNguoiDung == nguoiDungId && x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<bool> HasUserReviewedProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(x => x.MaNguoiDung == nguoiDungId && x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<bool> CanUserReviewProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        var hasReviewed = await HasUserReviewedProductAsync(nguoiDungId, sanPhamId, cancellationToken);
        if (hasReviewed) return false;

        return await _context.ChiTietDonHangs
            .Include(x => x.DonHang)
            .AnyAsync(x => x.DonHang.MaNguoiDung == nguoiDungId && 
                          x.MaSanPham == sanPhamId &&
                          x.DonHang.TrangThaiThanhToan == "DaThanhToan", cancellationToken);
    }

    public async Task<decimal> GetAverageRatingAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        var ratings = await _dbSet
            .Where(x => x.MaSanPham == sanPhamId && x.DaDuyet)
            .Select(x => x.DiemDanhGia)
            .ToListAsync(cancellationToken);

        return ratings.Any() ? (decimal)ratings.Average() : 0;
    }

    public async Task<int> GetReviewCountAsync(int sanPhamId, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaSanPham == sanPhamId);

        if (approvedOnly)
        {
            query = query.Where(x => x.DaDuyet);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<(int Rating, int Count)>> GetRatingDistributionAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        var distribution = await _dbSet
            .Where(x => x.MaSanPham == sanPhamId && x.DaDuyet)
            .GroupBy(x => x.DiemDanhGia)
            .Select(g => new { Rating = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Rating)
            .ToListAsync(cancellationToken);

        var result = new List<(int Rating, int Count)>();
        for (int i = 5; i >= 1; i--)
        {
            var item = distribution.FirstOrDefault(x => x.Rating == i);
            result.Add((i, item?.Count ?? 0));
        }

        return result;
    }

    public async Task<IEnumerable<DanhGiaSanPham>> GetPendingReviewsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.DonHang)
            .Where(x => !x.DaDuyet)
            .OrderBy(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ApproveReviewAsync(int danhGiaId, int nguoiDuyetId, CancellationToken cancellationToken = default)
    {
        var review = await GetByIdAsync(danhGiaId, cancellationToken);
        if (review == null) return false;

        review.DaDuyet = true;
        review.NgayDuyet = DateTime.UtcNow;
        review.NguoiDuyet = nguoiDuyetId;
        Update(review);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RejectReviewAsync(int danhGiaId, int nguoiDuyetId, CancellationToken cancellationToken = default)
    {
        var review = await GetByIdAsync(danhGiaId, cancellationToken);
        if (review == null) return false;

        review.DaDuyet = false;
        review.NgayDuyet = DateTime.UtcNow;
        review.NguoiDuyet = nguoiDuyetId;
        Update(review);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<(IEnumerable<DanhGiaSanPham> Reviews, int TotalCount)> GetPagedReviewsAsync(
        int sanPhamId,
        int pageNumber = 1,
        int pageSize = 10,
        int? rating = null,
        bool approvedOnly = true,
        string sortBy = "newest",
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.DonHang)
            .Where(x => x.MaSanPham == sanPhamId);

        if (approvedOnly)
        {
            query = query.Where(x => x.DaDuyet);
        }

        if (rating.HasValue && rating.Value >= 1 && rating.Value <= 5)
        {
            query = query.Where(x => x.DiemDanhGia == rating.Value);
        }

        query = sortBy.ToLower() switch
        {
            "oldest" => query.OrderBy(x => x.NgayTao),
            "highest" => query.OrderByDescending(x => x.DiemDanhGia).ThenByDescending(x => x.NgayTao),
            "lowest" => query.OrderBy(x => x.DiemDanhGia).ThenByDescending(x => x.NgayTao),
            _ => query.OrderByDescending(x => x.NgayTao)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var reviews = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (reviews, totalCount);
    }

    public async Task<IEnumerable<DanhGiaSanPham>> GetRecentReviewsAsync(int take = 10, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .AsQueryable();

        if (approvedOnly)
        {
            query = query.Where(x => x.DaDuyet);
        }

        return await query
            .OrderByDescending(x => x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhGiaSanPham>> GetTopReviewsAsync(int sanPhamId, int take = 5, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.DonHang)
            .Where(x => x.MaSanPham == sanPhamId && x.DaDuyet && x.LaMuaHangXacThuc)
            .OrderByDescending(x => x.DiemDanhGia)
            .ThenByDescending(x => x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhGiaSanPham>> GetReviewsByRatingAsync(int sanPhamId, int rating, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.DonHang)
            .Where(x => x.MaSanPham == sanPhamId && x.DiemDanhGia == rating);

        if (approvedOnly)
        {
            query = query.Where(x => x.DaDuyet);
        }

        return await query
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteUserReviewAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        var review = await GetUserReviewForProductAsync(nguoiDungId, sanPhamId, cancellationToken);
        if (review == null) return false;

        Remove(review);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> CountReviewsByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.MaNguoiDung == nguoiDungId, cancellationToken);
    }

    public async Task<IEnumerable<DanhGiaSanPham>> SearchReviewsAsync(string searchTerm, bool approvedOnly = true, CancellationToken cancellationToken = default)
    {
        var searchLower = searchTerm.ToLower();
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.SanPham)
            .Where(x => (x.TieuDe != null && x.TieuDe.ToLower().Contains(searchLower)) ||
                       (x.NoiDungDanhGia != null && x.NoiDungDanhGia.ToLower().Contains(searchLower)) ||
                       x.SanPham.TenSanPham.ToLower().Contains(searchLower));

        if (approvedOnly)
        {
            query = query.Where(x => x.DaDuyet);
        }

        return await query
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhGiaSanPham>> GetVerifiedPurchaseReviewsAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.DonHang)
            .Where(x => x.MaSanPham == sanPhamId && x.DaDuyet && x.LaMuaHangXacThuc)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<DanhGiaSanPham> Reviews, int TotalCount)> GetPagedReviewsForModerationAsync(
        int pageNumber = 1,
        int pageSize = 20,
        bool? isDuyet = null,
        int? rating = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiDung)
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.DonHang)
            .Include(x => x.NguoiDuyetNavigation)
            .AsQueryable();

        if (isDuyet.HasValue)
        {
            query = query.Where(x => x.DaDuyet == isDuyet.Value);
        }

        if (rating.HasValue && rating.Value >= 1 && rating.Value <= 5)
        {
            query = query.Where(x => x.DiemDanhGia == rating.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var reviews = await query
            .OrderBy(x => x.DaDuyet)
            .ThenBy(x => x.NgayTao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (reviews, totalCount);
    }
}