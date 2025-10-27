using BilliardShop.Application.Interfaces;
using BilliardShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using BilliardShop.Web.Controllers;
using System.Security.Claims;

namespace BilliardShop.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IProductService _productService;

    public CartController(ICartService cartService, IProductService productService)
    {
        _cartService = cartService;
        _productService = productService;
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

    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        var sessionId = GetSessionId();
        var cartItems = await _cartService.GetCartItemsAsync(userId, sessionId);

        var viewModel = new CartViewModel();

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
        viewModel.ShippingFee = 0; // Tính phí ship sau
        viewModel.Total = viewModel.SubTotal + viewModel.ShippingFee;

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var userId = GetCurrentUserId();
        var sessionId = GetSessionId();
        try
        {
            await _cartService.AddToCartAsync(userId, sessionId, productId, quantity);

            var cartItems = await _cartService.GetCartItemsAsync(userId, sessionId);
            var viewModel = new CartViewModel();

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

            return Json(new
            {
                success = true,
                message = "Đã thêm sản phẩm vào giỏ hàng",
                html = await this.RenderViewToStringAsync("_CartItems", viewModel),
                count = viewModel.Items.Count,
                subTotal = viewModel.SubTotal
            });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "Không thể thêm sản phẩm vào giỏ hàng" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        if (quantity <= 0)
        {
            return Json(new { success = false, message = "Số lượng không hợp lệ" });
        }

        try
        {
            var userId = GetCurrentUserId();
            var sessionId = GetSessionId();
            await _cartService.UpdateQuantityAsync(userId, sessionId, cartItemId, quantity);

            var total = await _cartService.GetCartTotalAsync(userId, sessionId);
            return Json(new { success = true, total });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "Không thể cập nhật số lượng" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userId = GetCurrentUserId();
        var sessionId = GetSessionId();
        try
        {
            await _cartService.RemoveFromCartAsync(userId, sessionId, productId);
            TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng";
        }
        catch (Exception)
        {
            TempData["Error"] = "Không thể xóa sản phẩm";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetCurrentUserId();
        var sessionId = GetSessionId();
        await _cartService.ClearCartAsync(userId, sessionId);
        TempData["Success"] = "Đã xóa toàn bộ giỏ hàng";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> GetCartCount()
    {
        var userId = GetCurrentUserId();
        var sessionId = GetSessionId();
        var count = await _cartService.GetCartItemCountAsync(userId, sessionId);
        return Json(new { count });
    }
}
