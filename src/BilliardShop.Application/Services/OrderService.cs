using BilliardShop.Application.Interfaces;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartService _cartService;

    public OrderService(IUnitOfWork unitOfWork, ICartService cartService)
    {
        _unitOfWork = unitOfWork;
        _cartService = cartService;
    }

    public async Task<DonHang?> CreateOrderAsync(int? nguoiDungId, string? maPhienLamViec, string tenNguoiNhan, string soDienThoai, string diaChi, string? ghiChu)
    {
        // Lấy giỏ hàng
        var cartItems = await _cartService.GetCartItemsAsync(nguoiDungId, maPhienLamViec);
        if (!cartItems.Any())
        {
            return null;
        }

        // Tính tổng tiền
        var tongTien = await _cartService.GetCartTotalAsync(nguoiDungId, maPhienLamViec);

        // Tạo đơn hàng
        var donHang = new DonHang
        {
            SoDonHang = $"DH{DateTime.Now:yyyyMMddHHmmss}",
            MaNguoiDung = nguoiDungId,
            TenKhachHang = tenNguoiNhan,
            SoDienThoaiKhachHang = soDienThoai,
            DiaChiGiaoHang = diaChi,
            GhiChuKhachHang = ghiChu,
            TongTienHang = tongTien,
            TongThanhToan = tongTien,
            MaTrangThai = 1, // Đang xử lý
            TrangThaiThanhToan = "ChoThanhToan",
            NgayDatHang = DateTime.Now
        };

        await _unitOfWork.DonHangRepository.AddAsync(donHang);
        await _unitOfWork.SaveChangesAsync();

        // Tạo chi tiết đơn hàng
        foreach (var item in cartItems)
        {
            var donGia = item.SanPham.GiaKhuyenMai ?? item.SanPham.GiaGoc;
            var chiTiet = new ChiTietDonHang
            {
                MaDonHang = donHang.Id,
                MaSanPham = item.MaSanPham,
                TenSanPham = item.SanPham.TenSanPham,
                SoLuong = item.SoLuong,
                DonGia = donGia,
                ThanhTien = donGia * item.SoLuong
            };

            await _unitOfWork.ChiTietDonHangRepository.AddAsync(chiTiet);

            // Cập nhật tồn kho
            var sanPham = await _unitOfWork.SanPhamRepository.GetByIdAsync(item.MaSanPham);
            if (sanPham != null)
            {
                sanPham.SoLuongTonKho -= item.SoLuong;
                _unitOfWork.SanPhamRepository.Update(sanPham);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        // Xóa giỏ hàng sau khi đặt hàng thành công
        await _cartService.ClearCartAsync(nguoiDungId, maPhienLamViec);

        return donHang;
    }

    public async Task<DonHang?> GetOrderByIdAsync(int orderId)
    {
        return await _unitOfWork.DonHangRepository.GetOrderWithDetailsAsync(orderId);
    }

    public async Task<IEnumerable<DonHang>> GetOrdersByUserAsync(int nguoiDungId)
    {
        return await _unitOfWork.DonHangRepository.GetByUserIdAsync(nguoiDungId);
    }
}
