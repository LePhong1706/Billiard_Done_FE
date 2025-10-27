using BilliardShop.Domain.Entities;

namespace BilliardShop.Web.Models;

public class ProductListViewModel
{
    public IEnumerable<SanPham> Products { get; set; } = new List<SanPham>();
    public DanhMucSanPham? CurrentCategory { get; set; }
    public IEnumerable<DanhMucSanPham> AllCategories { get; set; } = new List<DanhMucSanPham>();

    // Available filter options
    public IEnumerable<ThuongHieu> AvailableBrands { get; set; } = new List<ThuongHieu>();
    public IEnumerable<string> AvailableColors { get; set; } = new List<string>();
    public IEnumerable<string> AvailableMaterials { get; set; } = new List<string>();

    // Pagination
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalProducts { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalProducts / PageSize);

    // Filter properties
    public string? SearchTerm { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<string> SelectedBrandSlugs { get; set; } = new List<string>();
    public List<string> SelectedColors { get; set; } = new List<string>();
    public List<string> SelectedMaterials { get; set; } = new List<string>();
    public bool? InStockOnly { get; set; }
    public bool? FeaturedOnly { get; set; }
    public bool? OnSaleOnly { get; set; }
    public string? SortBy { get; set; }

    // Helper to check if any filters are active
    public bool HasActiveFilters =>
        !string.IsNullOrEmpty(SearchTerm) ||
        MinPrice.HasValue ||
        MaxPrice.HasValue ||
        SelectedBrandSlugs.Any() ||
        SelectedColors.Any() ||
        SelectedMaterials.Any() ||
        InStockOnly.HasValue ||
        FeaturedOnly.HasValue ||
        OnSaleOnly.HasValue;

    // Helper to get count of active filters
    public int ActiveFiltersCount
    {
        get
        {
            int count = 0;
            if (!string.IsNullOrEmpty(SearchTerm)) count++;
            if (MinPrice.HasValue || MaxPrice.HasValue) count++;
            count += SelectedBrandSlugs.Count;
            count += SelectedColors.Count;
            count += SelectedMaterials.Count;
            if (InStockOnly == true) count++;
            if (FeaturedOnly == true) count++;
            if (OnSaleOnly == true) count++;
            return count;
        }
    }
}
