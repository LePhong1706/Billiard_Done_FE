using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BilliardShop.Application.Interfaces;
using BilliardShop.Web.Models;

namespace BilliardShop.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderService orderService,
            ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _orderService.GetOrdersByUserAsync(userId);

            var viewModel = orders.Select(o => new OrderHistoryViewModel
            {
                OrderId = o.Id,
                OrderNumber = o.SoDonHang,
                OrderDate = o.NgayDatHang,
                TotalAmount = o.TongThanhToan,
                Status = GetStatusText(o.MaTrangThai),
                PaymentStatus = GetPaymentStatusText(o.TrangThaiThanhToan),
                ItemCount = o.ChiTietDonHangs?.Count ?? 0
            }).OrderByDescending(o => o.OrderDate).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            // Kiểm tra xem đơn hàng có thuộc về user hiện tại không
            if (order.MaNguoiDung != userId)
            {
                return Forbid();
            }

            var viewModel = new OrderDetailViewModel
            {
                OrderId = order.Id,
                OrderNumber = order.SoDonHang,
                OrderDate = order.NgayDatHang,
                TenNguoiNhan = order.TenKhachHang ?? string.Empty,
                SoDienThoai = order.SoDienThoaiKhachHang ?? string.Empty,
                DiaChiGiaoHang = order.DiaChiGiaoHang ?? string.Empty,
                GhiChu = order.GhiChuKhachHang,
                TongTienHang = order.TongTienHang,
                TienGiamGia = order.TienGiamGia,
                PhiVanChuyen = order.PhiVanChuyen,
                TongThanhToan = order.TongThanhToan,
                Status = GetStatusText(order.MaTrangThai),
                PaymentStatus = GetPaymentStatusText(order.TrangThaiThanhToan),
                Items = order.ChiTietDonHangs?.Select(item => new OrderDetailItemViewModel
                {
                    ProductId = item.MaSanPham,
                    ProductName = item.TenSanPham,
                    ProductCode = item.MaCodeSanPham ?? string.Empty,
                    Quantity = item.SoLuong,
                    UnitPrice = item.DonGia,
                    TotalPrice = item.ThanhTien
                }).ToList() ?? new List<OrderDetailItemViewModel>()
            };

            return View(viewModel);
        }

        private static string GetStatusText(int statusId)
        {
            return statusId switch
            {
                1 => "Chờ duyệt",
                2 => "Đang xử lý",
                3 => "Đã đóng gói",
                4 => "Đang giao",
                5 => "Đã giao",
                6 => "Đã hủy",
                7 => "Hoàn trả",
                _ => "Không xác định"
            };
        }

        private static string GetPaymentStatusText(string? status)
        {
            return status switch
            {
                "ChoThanhToan" => "Chờ thanh toán",
                "DaThanhToan" => "Đã thanh toán",
                "ThatBai" => "Thanh toán thất bại",
                "HoanTien" => "Đã hoàn tiền",
                _ => "Không xác định"
            };
        }
    }
}
