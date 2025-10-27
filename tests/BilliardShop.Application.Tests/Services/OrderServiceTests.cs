using BilliardShop.Application.Interfaces;
using BilliardShop.Application.Services;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;
using Moq;
using Xunit;

namespace BilliardShop.Application.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICartService> _cartServiceMock;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _cartServiceMock = new Mock<ICartService>();
        _orderService = new OrderService(_unitOfWorkMock.Object, _cartServiceMock.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_WithEmptyCart_ReturnsNull()
    {
        // Arrange
        _cartServiceMock.Setup(x => x.GetCartItemsAsync(null, "session123"))
            .ReturnsAsync(new List<GioHang>());

        // Act
        var result = await _orderService.CreateOrderAsync(null, "session123", "Nguyen Van A", "0123456789", "Ha Noi", null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateOrderAsync_WithValidCart_CreatesOrderSuccessfully()
    {
        // Arrange
        var sessionId = "session123";
        var cartItems = new List<GioHang>
        {
            new GioHang
            {
                Id = 1,
                MaSanPham = 1,
                SoLuong = 2,
                SanPham = new SanPham
                {
                    Id = 1,
                    TenSanPham = "Cơ bida",
                    GiaGoc = 1000000,
                    GiaKhuyenMai = null,
                    SoLuongTonKho = 10
                }
            }
        };

        var expectedTotal = 2000000m;

        _cartServiceMock.Setup(x => x.GetCartItemsAsync(null, sessionId))
            .ReturnsAsync(cartItems);

        _cartServiceMock.Setup(x => x.GetCartTotalAsync(null, sessionId))
            .ReturnsAsync(expectedTotal);

        _unitOfWorkMock.Setup(x => x.DonHangRepository.AddAsync(It.IsAny<DonHang>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.ChiTietDonHangRepository.AddAsync(It.IsAny<ChiTietDonHang>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.SanPhamRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(cartItems[0].SanPham);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        _cartServiceMock.Setup(x => x.ClearCartAsync(null, sessionId))
            .ReturnsAsync(true);

        // Act
        var result = await _orderService.CreateOrderAsync(null, sessionId, "Nguyen Van A", "0123456789", "Ha Noi", "Giao hang nhanh");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Nguyen Van A", result.TenKhachHang);
        Assert.Equal("0123456789", result.SoDienThoaiKhachHang);
        Assert.Equal("Ha Noi", result.DiaChiGiaoHang);
        Assert.Equal("Giao hang nhanh", result.GhiChuKhachHang);
        Assert.Equal(expectedTotal, result.TongThanhToan);
        Assert.Equal("ChoThanhToan", result.TrangThaiThanhToan);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.AtLeastOnce);
        _cartServiceMock.Verify(x => x.ClearCartAsync(null, sessionId), Times.Once);
    }

    [Fact]
    public async Task GetOrderByIdAsync_WithValidId_ReturnsOrder()
    {
        // Arrange
        var orderId = 1;
        var expectedOrder = new DonHang
        {
            Id = orderId,
            TenKhachHang = "Nguyen Van A",
            TongThanhToan = 2000000
        };

        _unitOfWorkMock.Setup(x => x.DonHangRepository.GetByIdAsync(orderId))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _orderService.GetOrderByIdAsync(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        Assert.Equal("Nguyen Van A", result.TenKhachHang);
    }
}
