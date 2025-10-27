using BilliardShop.Domain.Entities;

namespace BilliardShop.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<SanPham>> GetFeaturedProductsAsync(int count = 10);
    Task<SanPham?> GetProductBySlugAsync(string productSlug);
    Task<IEnumerable<DanhMucSanPham>> GetAllCategoriesAsync();
    Task<DanhMucSanPham?> GetCategoryBySlugAsync(string categorySlug);

    // Unified filtering method with multiple filters
    Task<IEnumerable<SanPham>> GetProductsAsync(
        string? searchTerm = null,
        string? categorySlug = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        List<string>? brandSlugs = null,
        List<string>? colors = null,
        List<string>? materials = null,
        bool? inStock = null,
        bool? isFeatured = null,
        bool? isOnSale = null,
        string? sortBy = null,
        int pageNumber = 1,
        int pageSize = 12);

    Task<int> GetProductsCountAsync(
        string? searchTerm = null,
        string? categorySlug = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        List<string>? brandSlugs = null,
        List<string>? colors = null,
        List<string>? materials = null,
        bool? inStock = null,
        bool? isFeatured = null,
        bool? isOnSale = null);

    // Get distinct values for filters
    Task<IEnumerable<ThuongHieu>> GetAllBrandsAsync();
    Task<IEnumerable<string>> GetAllColorsAsync();
    Task<IEnumerable<string>> GetAllMaterialsAsync();
}
