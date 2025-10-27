using BilliardShop.Application.Interfaces;
using BilliardShop.Application.Interfaces.Services;
using BilliardShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BilliardShop.Web.Controllers;

public class CheckoutController : Controller
{
    private readonly ICartService _cartService;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly INguoiDungService _nguoiDungService;

    public CheckoutController(
        ICartService cartService,
        IProductService productService,
        IOrderService orderService,
        INguoiDungService nguoiDungService)
    {
        _cartService = cartService;
        _productService = productService;
        _orderService = orderService;
        _nguoiDungService = nguoiDungService;
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }
        return null;
    }

    private string GetSessionId()
    {
        var sessionId = HttpContext.Session.GetString("CartSessionId");
        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("CartSessionId", sessionId);
        }
        return sessionId;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        var sessionId = GetSessionId();
        var cartItems = await _cartService.GetCartItemsAsync(userId, sessionId);

        if (!cartItems.Any())
        {
            TempData["Error"] = "Giỏ hàng của bạn đang trống";
            return RedirectToAction("Index", "Cart");
        }

        var viewModel = new CheckoutViewModel();

        // Nếu user đã đăng nhập, điền sẵn thông tin
        if (userId.HasValue)
        {
            var user = await _nguoiDungService.GetUserWithDetailsAsync(userId.Value);
            if (user != null)
            {
                viewModel.TenNguoiNhan = $"{user.Ho} {user.Ten}".Trim();
                viewModel.SoDienThoai = user.SoDienThoai ?? string.Empty;
            }
        }

        foreach (var item in cartItems)
        {
            var product = await _productService.GetProductBySlugAsync(item.SanPham.DuongDanSanPham);
            if (product != null)
            {
                var cartItemVM = new CartItemViewModel
                {
                    GioHangId = item.Id,
                    SanPhamId = item.MaSanPham,
                    TenSanPham = product.TenSanPham,
                    DuongDanSanPham = product.DuongDanSanPham,
                    HinhAnhUrl = product.HinhAnhs.FirstOrDefault()?.DuongDanHinhAnh,
                    Gia = product.GiaGoc,
                    GiaKhuyenMai = product.GiaKhuyenMai,
                    SoLuong = item.SoLuong,
                    SoLuongTonKho = product.SoLuongTonKho
                };
                viewModel.Items.Add(cartItemVM);
            }
        }

        viewModel.SubTotal = viewModel.Items.Sum(i => i.ThanhTien);
        viewModel.ShippingFee = 0; // Miễn phí ship
        viewModel.Total = viewModel.SubTotal + viewModel.ShippingFee;

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Vui lòng điền đầy đủ thông tin";
            return RedirectToAction("Index");
        }

        var userId = GetCurrentUserId();
        var sessionId = GetSessionId();

        var order = await _orderService.CreateOrderAsync(
            userId,
            sessionId,
            model.TenNguoiNhan,
            model.SoDienThoai,
            model.DiaChiGiaoHang,
            model.GhiChu
        );

        if (order == null)
        {
            TempData["Error"] = "Không thể tạo đơn hàng. Vui lòng thử lại";
            return RedirectToAction("Index");
        }

        TempData["SuccessMessage"] = "Đặt hàng thành công! Cảm ơn bạn đã mua hàng.";
        return RedirectToAction("Confirmation", new { orderId = order.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Confirmation(int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            TempData["Error"] = "Không tìm thấy đơn hàng";
            return RedirectToAction("Index", "Home");
        }

        var viewModel = new OrderConfirmationViewModel
        {
            OrderId = order.Id,
            OrderNumber = order.SoDonHang,
            OrderDate = order.NgayDatHang,
            TenNguoiNhan = order.TenKhachHang ?? "",
            SoDienThoai = order.SoDienThoaiKhachHang ?? "",
            DiaChiGiaoHang = order.DiaChiGiaoHang ?? "",
            GhiChu = order.GhiChuKhachHang,
            TongTien = order.TongThanhToan,
            TrangThaiDonHang = order.TrangThaiThanhToan
        };

        foreach (var item in order.ChiTietDonHangs)
        {
            viewModel.Items.Add(new OrderItemViewModel
            {
                TenSanPham = item.TenSanPham,
                SoLuong = item.SoLuong,
                Gia = item.DonGia,
                ThanhTien = item.ThanhTien
            });
        }

        return View(viewModel);
    }
}
