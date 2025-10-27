using BilliardShop.Application.Interfaces;
using BilliardShop.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilliardShop.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(
        string? categorySlug,
        string? q,
        decimal? minPrice,
        decimal? maxPrice,
        List<string>? brands,
        List<string>? colors,
        List<string>? materials,
        bool? inStock,
        bool? featured,
        bool? onSale,
        string? sortBy,
        int page = 1)
    {
        // Initialize filter lists if null
        brands ??= new List<string>();
        colors ??= new List<string>();
        materials ??= new List<string>();

        var viewModel = new ProductListViewModel
        {
            CurrentPage = page,
            PageSize = 12,
            SearchTerm = q,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            SelectedBrandSlugs = brands,
            SelectedColors = colors,
            SelectedMaterials = materials,
            InStockOnly = inStock,
            FeaturedOnly = featured,
            OnSaleOnly = onSale,
            SortBy = sortBy
        };

        // Get category info if specified
        if (!string.IsNullOrEmpty(categorySlug))
        {
            viewModel.CurrentCategory = await _productService.GetCategoryBySlugAsync(categorySlug);
            if (viewModel.CurrentCategory == null)
            {
                return NotFound();
            }
        }

        // Get products with all filters applied
        viewModel.Products = await _productService.GetProductsAsync(
            searchTerm: q,
            categorySlug: categorySlug,
            minPrice: minPrice,
            maxPrice: maxPrice,
            brandSlugs: brands,
            colors: colors,
            materials: materials,
            inStock: inStock,
            isFeatured: featured,
            isOnSale: onSale,
            sortBy: sortBy,
            pageNumber: page,
            pageSize: viewModel.PageSize
        );

        viewModel.TotalProducts = await _productService.GetProductsCountAsync(
            searchTerm: q,
            categorySlug: categorySlug,
            minPrice: minPrice,
            maxPrice: maxPrice,
            brandSlugs: brands,
            colors: colors,
            materials: materials,
            inStock: inStock,
            isFeatured: featured,
            isOnSale: onSale
        );

        // Load all filter options
        viewModel.AllCategories = await _productService.GetAllCategoriesAsync();
        viewModel.AvailableBrands = await _productService.GetAllBrandsAsync();
        viewModel.AvailableColors = await _productService.GetAllColorsAsync();
        viewModel.AvailableMaterials = await _productService.GetAllMaterialsAsync();

        return View(viewModel);
    }

    public async Task<IActionResult> Details(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var product = await _productService.GetProductBySlugAsync(slug);
        if (product == null)
        {
            return NotFound();
        }

        var viewModel = new ProductDetailViewModel
        {
            Product = product,
            RelatedProducts = await _productService.GetProductsAsync(
                categorySlug: product.DanhMuc.DuongDanDanhMuc,
                pageNumber: 1,
                pageSize: 4
            )
        };

        // Remove the current product from related products
        viewModel.RelatedProducts = viewModel.RelatedProducts
            .Where(p => p.Id != product.Id)
            .Take(3);

        return View(viewModel);
    }
}
