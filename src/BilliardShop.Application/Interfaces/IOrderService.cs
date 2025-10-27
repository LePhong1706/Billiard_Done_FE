using BilliardShop.Domain.Entities;

namespace BilliardShop.Application.Interfaces;

public interface IOrderService
{
    Task<DonHang?> CreateOrderAsync(int? nguoiDungId, string? maPhienLamViec, string tenNguoiNhan, string soDienThoai, string diaChi, string? ghiChu);
    Task<DonHang?> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<DonHang>> GetOrdersByUserAsync(int nguoiDungId);
}
