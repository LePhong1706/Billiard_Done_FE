using BilliardShop.Domain.Entities;

namespace BilliardShop.Application.Interfaces;

public interface ICartService
{
    Task<IEnumerable<GioHang>> GetCartItemsAsync(int? nguoiDungId, string? maPhienLamViec);
    Task AddToCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId, int soLuong = 1);
    Task UpdateQuantityAsync(int? nguoiDungId, string? maPhienLamViec, int gioHangId, int newQuantity);
    Task RemoveFromCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId);
    Task<int> GetCartItemCountAsync(int? nguoiDungId, string? maPhienLamViec);
    Task<decimal> GetCartTotalAsync(int? nguoiDungId, string? maPhienLamViec);
    Task<bool> ClearCartAsync(int? nguoiDungId, string? maPhienLamViec);
}
