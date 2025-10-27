using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class SanPhamRepository : GenericRepository<SanPham>, ISanPhamRepository
{
    public SanPhamRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<SanPham?> GetBySlugAsync(string duongDanSanPham, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs)
            .Include(x => x.ThuocTinhs)
            .FirstOrDefaultAsync(x => x.DuongDanSanPham == duongDanSanPham && x.TrangThaiHoatDong, cancellationToken);
    }

    public async Task<SanPham?> GetByCodeAsync(string maCodeSanPham, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .FirstOrDefaultAsync(x => x.MaCodeSanPham == maCodeSanPham && x.TrangThaiHoatDong, cancellationToken);
    }

    public async Task<SanPham?> GetDetailAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.OrderBy(h => h.ThuTuSapXep))
            .Include(x => x.ThuocTinhs)
            .Include(x => x.DanhGias.Where(d => d.DaDuyet))
            .FirstOrDefaultAsync(x => x.Id == sanPhamId, cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => x.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetFeaturedAsync(int take = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => x.TrangThaiHoatDong && x.LaSanPhamNoiBat)
            .OrderByDescending(x => x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetOnSaleAsync(int take = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => x.TrangThaiHoatDong && x.GiaKhuyenMai.HasValue && x.GiaKhuyenMai < x.GiaGoc)
            .OrderByDescending(x => (x.GiaGoc - x.GiaKhuyenMai) / x.GiaGoc)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetNewArrivalsAsync(int days = 30, int take = 10, CancellationToken cancellationToken = default)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => x.TrangThaiHoatDong && x.NgayTao >= fromDate)
            .OrderByDescending(x => x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetByCategoryAsync(int categoryId, bool includeSubcategories = false, CancellationToken cancellationToken = default)
    {
        if (!includeSubcategories)
        {
            return await _dbSet
                .Include(x => x.DanhMuc)
                .Include(x => x.ThuongHieu)
                .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
                .Where(x => x.MaDanhMuc == categoryId && x.TrangThaiHoatDong)
                .OrderByDescending(x => x.NgayTao)
                .ToListAsync(cancellationToken);
        }

        var categoryIds = new List<int> { categoryId };
        await GetAllSubcategoryIds(categoryId, categoryIds, cancellationToken);

        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => categoryIds.Contains(x.MaDanhMuc) && x.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetByBrandAsync(int brandId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => x.MaThuongHieu == brandId && x.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetRelatedAsync(int sanPhamId, int take = 5, CancellationToken cancellationToken = default)
    {
        var product = await _dbSet
            .Where(x => x.Id == sanPhamId)
            .Select(x => new { x.MaDanhMuc, x.MaThuongHieu })
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null) return Enumerable.Empty<SanPham>();

        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => x.Id != sanPhamId && x.TrangThaiHoatDong &&
                       (x.MaDanhMuc == product.MaDanhMuc || x.MaThuongHieu == product.MaThuongHieu))
            .OrderByDescending(x => x.MaDanhMuc == product.MaDanhMuc ? 2 : 1)
            .ThenByDescending(x => x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetLowStockAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Where(x => x.TrangThaiHoatDong && x.SoLuongTonKho <= x.SoLuongToiThieu && x.SoLuongTonKho > 0)
            .OrderBy(x => x.SoLuongTonKho)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetOutOfStockAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Where(x => x.TrangThaiHoatDong && x.SoLuongTonKho == 0)
            .OrderByDescending(x => x.NgayCapNhatCuoi)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsBySlugAsync(string duongDanSanPham, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.DuongDanSanPham == duongDanSanPham);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string maCodeSanPham, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaCodeSanPham == maCodeSanPham);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> IsInStockAsync(int sanPhamId, int quantity = 1, CancellationToken cancellationToken = default)
    {
        var stock = await _dbSet
            .Where(x => x.Id == sanPhamId && x.TrangThaiHoatDong)
            .Select(x => x.SoLuongTonKho)
            .FirstOrDefaultAsync(cancellationToken);

        return stock >= quantity;
    }

    public async Task<bool> UpdateStockAsync(int sanPhamId, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(sanPhamId, cancellationToken);
        if (product == null) return false;

        product.SoLuongTonKho += quantity;
        if (product.SoLuongTonKho < 0) product.SoLuongTonKho = 0;

        Update(product);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> ReserveStockAsync(int sanPhamId, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(sanPhamId, cancellationToken);
        if (product == null || product.SoLuongTonKho < quantity) return false;

        product.SoLuongTonKho -= quantity;
        Update(product);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<decimal?> GetCurrentPriceAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        var product = await _dbSet
            .Where(x => x.Id == sanPhamId && x.TrangThaiHoatDong)
            .Select(x => new { x.GiaGoc, x.GiaKhuyenMai })
            .FirstOrDefaultAsync(cancellationToken);

        return product?.GiaKhuyenMai ?? product?.GiaGoc;
    }

    public async Task<(IEnumerable<SanPham> Products, int TotalCount)> SearchAsync(
        string? searchTerm = null,
        int? categoryId = null,
        int? brandId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? isOnSale = null,
        bool? isFeatured = null,
        bool? inStock = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => x.TrangThaiHoatDong);

        // Search term filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(x =>
                x.TenSanPham.ToLower().Contains(searchLower) ||
                x.MaCodeSanPham.ToLower().Contains(searchLower) ||
                (x.MoTaNgan != null && x.MoTaNgan.ToLower().Contains(searchLower)) ||
                (x.DanhMuc.TenDanhMuc.ToLower().Contains(searchLower)) ||
                (x.ThuongHieu != null && x.ThuongHieu.TenThuongHieu.ToLower().Contains(searchLower))
            );
        }

        // Category filter
        if (categoryId.HasValue)
        {
            var categoryIds = new List<int> { categoryId.Value };
            await GetAllSubcategoryIds(categoryId.Value, categoryIds, cancellationToken);
            query = query.Where(x => categoryIds.Contains(x.MaDanhMuc));
        }

        // Brand filter
        if (brandId.HasValue)
        {
            query = query.Where(x => x.MaThuongHieu == brandId.Value);
        }

        // Price range filter
        if (minPrice.HasValue || maxPrice.HasValue)
        {
            query = query.Where(x =>
                (!minPrice.HasValue || (x.GiaKhuyenMai ?? x.GiaGoc) >= minPrice.Value) &&
                (!maxPrice.HasValue || (x.GiaKhuyenMai ?? x.GiaGoc) <= maxPrice.Value)
            );
        }

        // Sale filter
        if (isOnSale.HasValue)
        {
            if (isOnSale.Value)
            {
                query = query.Where(x => x.GiaKhuyenMai.HasValue && x.GiaKhuyenMai < x.GiaGoc);
            }
            else
            {
                query = query.Where(x => !x.GiaKhuyenMai.HasValue || x.GiaKhuyenMai >= x.GiaGoc);
            }
        }

        // Featured filter
        if (isFeatured.HasValue)
        {
            query = query.Where(x => x.LaSanPhamNoiBat == isFeatured.Value);
        }

        // Stock filter
        if (inStock.HasValue)
        {
            if (inStock.Value)
            {
                query = query.Where(x => x.SoLuongTonKho > 0);
            }
            else
            {
                query = query.Where(x => x.SoLuongTonKho == 0);
            }
        }

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "name" => sortDescending ? query.OrderByDescending(x => x.TenSanPham) : query.OrderBy(x => x.TenSanPham),
            "price" => sortDescending ? query.OrderByDescending(x => x.GiaKhuyenMai ?? x.GiaGoc) : query.OrderBy(x => x.GiaKhuyenMai ?? x.GiaGoc),
            "created" => sortDescending ? query.OrderByDescending(x => x.NgayTao) : query.OrderBy(x => x.NgayTao),
            "updated" => sortDescending ? query.OrderByDescending(x => x.NgayCapNhatCuoi) : query.OrderBy(x => x.NgayCapNhatCuoi),
            _ => query.OrderByDescending(x => x.NgayTao)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (products, totalCount);
    }

    public async Task<IEnumerable<SanPham>> GetTopSellingAsync(int days = 30, int take = 10, CancellationToken cancellationToken = default)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);

        return await _context.ChiTietDonHangs
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.DonHang)
            .Where(x => x.DonHang.NgayDatHang >= fromDate && x.SanPham.TrangThaiHoatDong)
            .GroupBy(x => x.MaSanPham)
            .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(x => x.SoLuong) })
            .OrderByDescending(x => x.TotalSold)
            .Take(take)
            .Join(_dbSet,
                  x => x.ProductId,
                  p => p.Id,
                  (x, p) => p)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<(string AttributeName, string AttributeValue, int Count)>> GetAvailableFiltersAsync(
        int? categoryId = null,
        int? brandId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TrangThaiHoatDong);

        if (categoryId.HasValue)
        {
            query = query.Where(x => x.MaDanhMuc == categoryId.Value);
        }

        if (brandId.HasValue)
        {
            query = query.Where(x => x.MaThuongHieu == brandId.Value);
        }

        var attributes = await query
            .SelectMany(x => x.ThuocTinhs)
            .GroupBy(x => new { x.TenThuocTinh, x.GiaTriThuocTinh })
            .Select(g => new { g.Key.TenThuocTinh, g.Key.GiaTriThuocTinh, Count = g.Count() })
            .OrderBy(x => x.TenThuocTinh)
            .ThenBy(x => x.GiaTriThuocTinh)
            .ToListAsync(cancellationToken);

        return attributes.Select(x => (x.TenThuocTinh, x.GiaTriThuocTinh ?? "", x.Count));
    }

    public async Task<decimal> GetAverageRatingAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        var ratings = await _context.DanhGiaSanPhams
            .Where(x => x.MaSanPham == sanPhamId && x.DaDuyet)
            .Select(x => x.DiemDanhGia)
            .ToListAsync(cancellationToken);

        return ratings.Any() ? (decimal)ratings.Average() : 0;
    }

    public async Task<int> GetReviewCountAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _context.DanhGiaSanPhams
            .CountAsync(x => x.MaSanPham == sanPhamId && x.DaDuyet, cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => x.TrangThaiHoatDong &&
                       (x.GiaKhuyenMai ?? x.GiaGoc) >= minPrice &&
                       (x.GiaKhuyenMai ?? x.GiaGoc) <= maxPrice)
            .OrderBy(x => x.GiaKhuyenMai ?? x.GiaGoc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SanPham>> GetRecentlyViewedAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default)
    {
        var ids = productIds.ToList();
        if (!ids.Any()) return Enumerable.Empty<SanPham>();

        var products = await _dbSet
            .Include(x => x.DanhMuc)
            .Include(x => x.ThuongHieu)
            .Include(x => x.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => ids.Contains(x.Id) && x.TrangThaiHoatDong)
            .ToListAsync(cancellationToken);

        return ids.Select(id => products.FirstOrDefault(p => p.Id == id))
                 .Where(p => p != null)
                 .Cast<SanPham>();
    }

    private async Task GetAllSubcategoryIds(int parentId, List<int> categoryIds, CancellationToken cancellationToken)
    {
        var subcategories = await _context.DanhMucSanPhams
            .Where(x => x.MaDanhMucCha == parentId)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var subcategoryId in subcategories)
        {
            if (!categoryIds.Contains(subcategoryId))
            {
                categoryIds.Add(subcategoryId);
                await GetAllSubcategoryIds(subcategoryId, categoryIds, cancellationToken);
            }
        }
    }
}