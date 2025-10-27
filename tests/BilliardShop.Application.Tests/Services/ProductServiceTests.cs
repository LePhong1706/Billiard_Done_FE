using BilliardShop.Application.Services;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;
using Moq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace BilliardShop.Application.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<SanPham>> _mockProductRepository;
    private readonly Mock<IGenericRepository<DanhMucSanPham>> _mockCategoryRepository;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IGenericRepository<SanPham>>();
        _mockCategoryRepository = new Mock<IGenericRepository<DanhMucSanPham>>();

        _mockUnitOfWork.Setup(u => u.Repository<SanPham>()).Returns(_mockProductRepository.Object);
        _mockUnitOfWork.Setup(u => u.Repository<DanhMucSanPham>()).Returns(_mockCategoryRepository.Object);

        _productService = new ProductService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetFeaturedProductsAsync_ReturnsCorrectCount()
    {
        // Arrange
        var featuredProducts = new List<SanPham>
        {
            new SanPham { Id = 1, TenSanPham = "Product 1", TrangThaiHoatDong = true, LaSanPhamNoiBat = true, HinhAnhs = new List<HinhAnhSanPham>() },
            new SanPham { Id = 2, TenSanPham = "Product 2", TrangThaiHoatDong = true, LaSanPhamNoiBat = true, HinhAnhs = new List<HinhAnhSanPham>() },
            new SanPham { Id = 3, TenSanPham = "Product 3", TrangThaiHoatDong = true, LaSanPhamNoiBat = true, HinhAnhs = new List<HinhAnhSanPham>() }
        };

        _mockProductRepository.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<SanPham, bool>>>(),
            It.IsAny<Func<IQueryable<SanPham>, IOrderedQueryable<SanPham>>>(),
            It.IsAny<Func<IQueryable<SanPham>, IIncludableQueryable<SanPham, object>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>()
        )).ReturnsAsync(featuredProducts);

        // Act
        var result = await _productService.GetFeaturedProductsAsync(2);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetProductBySlugAsync_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        var slug = "test-product";
        var expectedProduct = new SanPham
        {
            Id = 1,
            TenSanPham = "Test Product",
            DuongDanSanPham = slug,
            TrangThaiHoatDong = true,
            HinhAnhs = new List<HinhAnhSanPham>(),
            DanhMuc = new DanhMucSanPham(),
            ThuocTinhs = new List<ThuocTinhSanPham>(),
            DanhGias = new List<DanhGiaSanPham>()
        };

        _mockProductRepository.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<SanPham, bool>>>(),
            It.IsAny<Func<IQueryable<SanPham>, IOrderedQueryable<SanPham>>>(),
            It.IsAny<Func<IQueryable<SanPham>, IIncludableQueryable<SanPham, object>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>()
        )).ReturnsAsync(new List<SanPham> { expectedProduct });

        // Act
        var result = await _productService.GetProductBySlugAsync(slug);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(slug, result.DuongDanSanPham);
    }

    [Fact]
    public async Task GetProductBySlugAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        // Arrange
        var slug = "non-existent-product";

        _mockProductRepository.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<SanPham, bool>>>(),
            It.IsAny<Func<IQueryable<SanPham>, IOrderedQueryable<SanPham>>>(),
            It.IsAny<Func<IQueryable<SanPham>, IIncludableQueryable<SanPham, object>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>()
        )).ReturnsAsync(new List<SanPham>());

        // Act
        var result = await _productService.GetProductBySlugAsync(slug);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ReturnsCategories()
    {
        // Arrange
        var categories = new List<DanhMucSanPham>
        {
            new DanhMucSanPham { Id = 1, TenDanhMuc = "Category 1", TrangThaiHoatDong = true },
            new DanhMucSanPham { Id = 2, TenDanhMuc = "Category 2", TrangThaiHoatDong = true }
        };

        _mockCategoryRepository.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<DanhMucSanPham, bool>>>(),
            It.IsAny<Func<IQueryable<DanhMucSanPham>, IOrderedQueryable<DanhMucSanPham>>>(),
            It.IsAny<Func<IQueryable<DanhMucSanPham>, IIncludableQueryable<DanhMucSanPham, object>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>()
        )).ReturnsAsync(categories);

        // Act
        var result = await _productService.GetAllCategoriesAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchProductsAsync_ReturnsMatchingProducts()
    {
        // Arrange
        var searchTerm = "Cơ";
        var products = new List<SanPham>
        {
            new SanPham { Id = 1, TenSanPham = "Cơ Bida", MoTaNgan = "Cơ chơi bida chất lượng", TrangThaiHoatDong = true, GiaGoc = 100000, HinhAnhs = new List<HinhAnhSanPham>(), DanhMuc = new DanhMucSanPham() },
            new SanPham { Id = 2, TenSanPham = "Phấn Bida", MoTaNgan = "Phấn cao cấp", TrangThaiHoatDong = true, GiaGoc = 50000, HinhAnhs = new List<HinhAnhSanPham>(), DanhMuc = new DanhMucSanPham() }
        };

        _mockProductRepository.Setup(r => r.GetPagedAsync(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<Expression<Func<SanPham, bool>>>(),
            It.IsAny<Func<IQueryable<SanPham>, IOrderedQueryable<SanPham>>>(),
            It.IsAny<Expression<Func<SanPham, object>>[]>()
        )).ReturnsAsync((products.Take(1), 1));

        // Act
        var result = await _productService.SearchProductsAsync(searchTerm);

        // Assert
        Assert.Single(result);
        Assert.Contains("Cơ", result.First().TenSanPham);
    }

    [Fact]
    public async Task SearchProductsAsync_WithPriceFilter_ReturnsFilteredProducts()
    {
        // Arrange
        var searchTerm = "Bida";
        decimal minPrice = 50000;
        decimal maxPrice = 150000;

        var products = new List<SanPham>
        {
            new SanPham { Id = 1, TenSanPham = "Cơ Bida", MoTaNgan = "Cơ chơi bida", TrangThaiHoatDong = true, GiaGoc = 100000, HinhAnhs = new List<HinhAnhSanPham>(), DanhMuc = new DanhMucSanPham() }
        };

        _mockProductRepository.Setup(r => r.GetPagedAsync(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<Expression<Func<SanPham, bool>>>(),
            It.IsAny<Func<IQueryable<SanPham>, IOrderedQueryable<SanPham>>>(),
            It.IsAny<Expression<Func<SanPham, object>>[]>()
        )).ReturnsAsync((products, 1));

        // Act
        var result = await _productService.SearchProductsAsync(searchTerm, null, minPrice, maxPrice);

        // Assert
        Assert.Single(result);
        Assert.True(result.First().GiaGoc >= minPrice && result.First().GiaGoc <= maxPrice);
    }

    [Fact]
    public async Task GetSearchResultsCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        var searchTerm = "Cơ";
        var expectedCount = 5;

        _mockProductRepository.Setup(r => r.CountAsync(
            It.IsAny<Expression<Func<SanPham, bool>>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(expectedCount);

        // Act
        var result = await _productService.GetSearchResultsCountAsync(searchTerm);

        // Assert
        Assert.Equal(expectedCount, result);
    }
}
