using BilliardShop.Application.Services;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;
using BilliardShop.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace BilliardShop.Application.Tests.Services;

public class CartServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGioHangRepository> _mockGioHangRepository;
    private readonly Mock<ISanPhamRepository> _mockSanPhamRepository;
    private readonly CartService _cartService;

    public CartServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockGioHangRepository = new Mock<IGioHangRepository>();
        _mockSanPhamRepository = new Mock<ISanPhamRepository>();

        _mockUnitOfWork.Setup(u => u.GioHangRepository).Returns(_mockGioHangRepository.Object);
        _mockUnitOfWork.Setup(u => u.SanPhamRepository).Returns(_mockSanPhamRepository.Object);

        _cartService = new CartService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetCartItemsAsync_WithUserId_ReturnsCartItems()
    {
        // Arrange
        var userId = 1;
        var expectedCartItems = new List<GioHang>
        {
            new GioHang { Id = 1, MaNguoiDung = userId, MaSanPham = 1, SoLuong = 2 },
            new GioHang { Id = 2, MaNguoiDung = userId, MaSanPham = 2, SoLuong = 1 }
        };

        _mockGioHangRepository
            .Setup(r => r.GetByUserIdAsync(userId, default))
            .ReturnsAsync(expectedCartItems);

        // Act
        var result = await _cartService.GetCartItemsAsync(userId, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockGioHangRepository.Verify(r => r.GetByUserIdAsync(userId, default), Times.Once);
    }

    [Fact]
    public async Task AddToCartAsync_ProductOutOfStock_ReturnsFalse()
    {
        // Arrange
        var sessionId = "test-session";
        var productId = 1;
        var quantity = 5;

        var product = new SanPham
        {
            Id = productId,
            TenSanPham = "Test Product",
            TrangThaiHoatDong = true,
            SoLuongTonKho = 2 // Less than requested quantity
        };

        _mockSanPhamRepository
            .Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _cartService.AddToCartAsync(null, sessionId, productId, quantity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddToCartAsync_ValidProduct_AddsToCart()
    {
        // Arrange
        var sessionId = "test-session";
        var productId = 1;
        var quantity = 2;

        var product = new SanPham
        {
            Id = productId,
            TenSanPham = "Test Product",
            TrangThaiHoatDong = true,
            SoLuongTonKho = 10
        };

        _mockSanPhamRepository
            .Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(product);

        _mockGioHangRepository
            .Setup(r => r.GetCartItemBySessionAsync(sessionId, productId, default))
            .ReturnsAsync((GioHang?)null);

        _mockGioHangRepository
            .Setup(r => r.AddAsync(It.IsAny<GioHang>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(default))
            .ReturnsAsync(1);

        // Act
        var result = await _cartService.AddToCartAsync(null, sessionId, productId, quantity);

        // Assert
        Assert.True(result);
        _mockGioHangRepository.Verify(r => r.AddAsync(It.Is<GioHang>(g =>
            g.MaPhienLamViec == sessionId &&
            g.MaSanPham == productId &&
            g.SoLuong == quantity)), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityAsync_InvalidQuantity_ReturnsFalse()
    {
        // Arrange
        var cartItemId = 1;
        var newQuantity = 0;

        // Act
        var result = await _cartService.UpdateQuantityAsync(cartItemId, newQuantity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RemoveFromCartAsync_ValidCartItem_RemovesItem()
    {
        // Arrange
        var sessionId = "test-session";
        var productId = 1;
        var cartItem = new GioHang
        {
            Id = 1,
            MaPhienLamViec = sessionId,
            MaSanPham = productId,
            SoLuong = 2
        };

        _mockGioHangRepository
            .Setup(r => r.GetCartItemBySessionAsync(sessionId, productId, default))
            .ReturnsAsync(cartItem);

        _mockGioHangRepository
            .Setup(r => r.DeleteAsync(cartItem.Id))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(default))
            .ReturnsAsync(1);

        // Act
        var result = await _cartService.RemoveFromCartAsync(null, sessionId, productId);

        // Assert
        Assert.True(result);
        _mockGioHangRepository.Verify(r => r.DeleteAsync(cartItem.Id), Times.Once);
    }
}
