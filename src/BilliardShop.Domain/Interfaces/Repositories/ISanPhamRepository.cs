using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface ISanPhamRepository : IGenericRepository<SanPham>
{
    Task<SanPham?> GetBySlugAsync(string duongDanSanPham, CancellationToken cancellationToken = default);
    Task<SanPham?> GetByCodeAsync(string maCodeSanPham, CancellationToken cancellationToken = default);
    Task<SanPham?> GetDetailAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetFeaturedAsync(int take = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetOnSaleAsync(int take = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetNewArrivalsAsync(int days = 30, int take = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetByCategoryAsync(int categoryId, bool includeSubcategories = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetByBrandAsync(int brandId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetRelatedAsync(int sanPhamId, int take = 5, CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetLowStockAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetOutOfStockAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsBySlugAsync(string duongDanSanPham, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string maCodeSanPham, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> IsInStockAsync(int sanPhamId, int quantity = 1, CancellationToken cancellationToken = default);
    Task<bool> UpdateStockAsync(int sanPhamId, int quantity, CancellationToken cancellationToken = default);
    Task<bool> ReserveStockAsync(int sanPhamId, int quantity, CancellationToken cancellationToken = default);
    Task<decimal?> GetCurrentPriceAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<SanPham> Products, int TotalCount)> SearchAsync(
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
        CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetTopSellingAsync(int days = 30, int take = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<(string AttributeName, string AttributeValue, int Count)>> GetAvailableFiltersAsync(
        int? categoryId = null,
        int? brandId = null,
        CancellationToken cancellationToken = default);
    Task<decimal> GetAverageRatingAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> GetReviewCountAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    Task<IEnumerable<SanPham>> GetRecentlyViewedAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default);
}