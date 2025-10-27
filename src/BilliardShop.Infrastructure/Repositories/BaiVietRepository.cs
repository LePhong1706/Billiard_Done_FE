using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class BaiVietRepository : GenericRepository<BaiViet>, IBaiVietRepository
{
    public BaiVietRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<BaiViet?> GetBySlugAsync(string duongDanBaiViet, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Include(x => x.BinhLuans.Where(c => c.TrangThai == "DaDuyet"))
            .FirstOrDefaultAsync(x => x.DuongDanBaiViet == duongDanBaiViet, cancellationToken);
    }

    public async Task<BaiViet?> GetPublishedBySlugAsync(string duongDanBaiViet, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Include(x => x.BinhLuans.Where(c => c.TrangThai == "DaDuyet"))
            .FirstOrDefaultAsync(x => x.DuongDanBaiViet == duongDanBaiViet && x.TrangThai == "XuatBan", cancellationToken);
    }

    public async Task<BaiViet?> GetWithDetailsAsync(int baiVietId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Include(x => x.BinhLuans.Where(c => c.TrangThai == "DaDuyet"))
                .ThenInclude(c => c.NguoiDung)
            .Include(x => x.BinhLuans.Where(c => c.TrangThai == "DaDuyet"))
                .ThenInclude(c => c.BinhLuanCons.Where(r => r.TrangThai == "DaDuyet"))
                    .ThenInclude(r => r.NguoiDung)
            .FirstOrDefaultAsync(x => x.Id == baiVietId, cancellationToken);
    }

    public async Task<IEnumerable<BaiViet>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.TrangThai == "XuatBan")
            .OrderByDescending(x => x.NgayXuatBan ?? x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BaiViet>> GetFeaturedAsync(int take = 5, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.TrangThai == "XuatBan" && x.NoiBat)
            .OrderByDescending(x => x.NgayXuatBan ?? x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BaiViet>> GetRecentAsync(int take = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.TrangThai == "XuatBan")
            .OrderByDescending(x => x.NgayXuatBan ?? x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BaiViet>> GetByAuthorAsync(int tacGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.TacGia == tacGiaId)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BaiViet>> GetByStatusAsync(string trangThai, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.TrangThai == trangThai)
            .OrderByDescending(x => x.NgayCapNhatCuoi ?? x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BaiViet>> GetDraftsByAuthorAsync(int tacGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.TacGia == tacGiaId && x.TrangThai == "NhapBan")
            .OrderByDescending(x => x.NgayCapNhatCuoi ?? x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BaiViet>> GetPendingPublishAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.TrangThai == "ChoXuatBan")
            .OrderBy(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsBySlugAsync(string duongDanBaiViet, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.DuongDanBaiViet == duongDanBaiViet);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> PublishAsync(int baiVietId, CancellationToken cancellationToken = default)
    {
        var article = await GetByIdAsync(baiVietId, cancellationToken);
        if (article == null) return false;

        article.TrangThai = "XuatBan";
        article.NgayXuatBan = DateTime.UtcNow;
        Update(article);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UnpublishAsync(int baiVietId, CancellationToken cancellationToken = default)
    {
        var article = await GetByIdAsync(baiVietId, cancellationToken);
        if (article == null) return false;

        article.TrangThai = "NhapBan";
        article.NgayXuatBan = null;
        Update(article);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> SetFeaturedAsync(int baiVietId, bool featured, CancellationToken cancellationToken = default)
    {
        var article = await GetByIdAsync(baiVietId, cancellationToken);
        if (article == null) return false;

        article.NoiBat = featured;
        Update(article);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> IncrementViewCountAsync(int baiVietId, CancellationToken cancellationToken = default)
    {
        var article = await GetByIdAsync(baiVietId, cancellationToken);
        if (article == null) return false;

        article.LuotXem++;
        Update(article);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<(IEnumerable<BaiViet> Articles, int TotalCount)> SearchAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Include(x => x.TacGiaNavigation).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(x =>
                x.TieuDe.ToLower().Contains(searchLower) ||
                (x.TomTat != null && x.TomTat.ToLower().Contains(searchLower)) ||
                (x.NoiDung != null && x.NoiDung.ToLower().Contains(searchLower)) ||
                (x.TuKhoaSEO != null && x.TuKhoaSEO.ToLower().Contains(searchLower))
            );
        }

        if (tacGiaId.HasValue)
        {
            query = query.Where(x => x.TacGia == tacGiaId.Value);
        }

        if (!string.IsNullOrWhiteSpace(trangThai))
        {
            query = query.Where(x => x.TrangThai == trangThai);
        }

        if (noiBat.HasValue)
        {
            query = query.Where(x => x.NoiBat == noiBat.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.NgayTao >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.NgayTao <= toDate.Value);
        }

        query = sortBy?.ToLower() switch
        {
            "title" => sortDescending ? query.OrderByDescending(x => x.TieuDe) : query.OrderBy(x => x.TieuDe),
            "views" => sortDescending ? query.OrderByDescending(x => x.LuotXem) : query.OrderBy(x => x.LuotXem),
            "published" => sortDescending ? query.OrderByDescending(x => x.NgayXuatBan) : query.OrderBy(x => x.NgayXuatBan),
            "updated" => sortDescending ? query.OrderByDescending(x => x.NgayCapNhatCuoi) : query.OrderBy(x => x.NgayCapNhatCuoi),
            _ => sortDescending ? query.OrderByDescending(x => x.NgayTao) : query.OrderBy(x => x.NgayTao)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var articles = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (articles, totalCount);
    }

    public async Task<IEnumerable<BaiViet>> GetMostViewedAsync(int days = 30, int take = 10, CancellationToken cancellationToken = default)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);

        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.TrangThai == "XuatBan" && (x.NgayXuatBan ?? x.NgayTao) >= fromDate)
            .OrderByDescending(x => x.LuotXem)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BaiViet>> GetRelatedAsync(int baiVietId, int take = 5, CancellationToken cancellationToken = default)
    {
        var article = await _dbSet
            .Where(x => x.Id == baiVietId)
            .Select(x => new { x.TuKhoaSEO, x.TacGia })
            .FirstOrDefaultAsync(cancellationToken);

        if (article == null) return Enumerable.Empty<BaiViet>();

        var query = _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.Id != baiVietId && x.TrangThai == "XuatBan");

        if (!string.IsNullOrWhiteSpace(article.TuKhoaSEO))
        {
            var keywords = article.TuKhoaSEO.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(k => k.Trim().ToLower())
                                          .Where(k => !string.IsNullOrEmpty(k))
                                          .ToList();

            if (keywords.Any())
            {
                query = query.Where(x => keywords.Any(k => 
                    (x.TuKhoaSEO != null && x.TuKhoaSEO.ToLower().Contains(k)) ||
                    x.TieuDe.ToLower().Contains(k) ||
                    (x.TomTat != null && x.TomTat.ToLower().Contains(k))
                ));
            }
        }

        var relatedArticles = await query
            .OrderByDescending(x => x.TacGia == article.TacGia ? 1 : 0)
            .ThenByDescending(x => x.NgayXuatBan ?? x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);

        if (relatedArticles.Count < take)
        {
            var additionalArticles = await _dbSet
                .Include(x => x.TacGiaNavigation)
                .Where(x => x.Id != baiVietId && 
                           x.TrangThai == "XuatBan" && 
                           !relatedArticles.Select(r => r.Id).Contains(x.Id))
                .OrderByDescending(x => x.NgayXuatBan ?? x.NgayTao)
                .Take(take - relatedArticles.Count)
                .ToListAsync(cancellationToken);

            relatedArticles.AddRange(additionalArticles);
        }

        return relatedArticles;
    }

    public async Task<int> CountByStatusAsync(string trangThai, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.TrangThai == trangThai, cancellationToken);
    }

    public async Task<int> CountByAuthorAsync(int tacGiaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.TacGia == tacGiaId, cancellationToken);
    }

    public async Task<IEnumerable<(DateTime Date, int Count)>> GetPublishStatsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.NgayXuatBan.HasValue && 
                       x.NgayXuatBan >= fromDate && 
                       x.NgayXuatBan <= toDate)
            .GroupBy(x => x.NgayXuatBan!.Value.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken)
            .ContinueWith(task => task.Result.Select(x => (x.Date, x.Count)), cancellationToken);
    }

    public async Task<bool> UpdateStatusAsync(int baiVietId, string trangThai, CancellationToken cancellationToken = default)
    {
        var article = await GetByIdAsync(baiVietId, cancellationToken);
        if (article == null) return false;

        article.TrangThai = trangThai;
        if (trangThai == "XuatBan" && !article.NgayXuatBan.HasValue)
        {
            article.NgayXuatBan = DateTime.UtcNow;
        }
        else if (trangThai != "XuatBan")
        {
            article.NgayXuatBan = null;
        }

        Update(article);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<string>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        var tags = await _dbSet
            .Where(x => !string.IsNullOrEmpty(x.TuKhoaSEO))
            .Select(x => x.TuKhoaSEO!)
            .ToListAsync(cancellationToken);

        return tags.SelectMany(t => t.Split(',', StringSplitOptions.RemoveEmptyEntries))
                  .Select(tag => tag.Trim())
                  .Where(tag => !string.IsNullOrEmpty(tag))
                  .Distinct()
                  .OrderBy(tag => tag);
    }

    public async Task<IEnumerable<BaiViet>> GetScheduledForPublishAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TacGiaNavigation)
            .Where(x => x.TrangThai == "ChoXuatBan" && 
                       x.NgayXuatBan.HasValue && 
                       x.NgayXuatBan <= DateTime.UtcNow)
            .OrderBy(x => x.NgayXuatBan)
            .ToListAsync(cancellationToken);
    }
}