using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IGioHangRepository : IGenericRepository<GioHang>
{
    Task<IEnumerable<GioHang>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GioHang>> GetBySessionIdAsync(string maPhienLamViec, CancellationToken cancellationToken = default);
    Task<GioHang?> GetCartItemAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<GioHang?> GetCartItemBySessionAsync(string maPhienLamViec, int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> CountItemsByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<int> CountItemsBySessionAsync(string maPhienLamViec, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalValueByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalValueBySessionAsync(string maPhienLamViec, CancellationToken cancellationToken = default);
    Task<bool> AddToCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId, int soLuong, CancellationToken cancellationToken = default);
    Task<bool> UpdateQuantityAsync(int gioHangId, int newQuantity, CancellationToken cancellationToken = default);
    Task<bool> RemoveFromCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> ClearCartByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<int> ClearCartBySessionAsync(string maPhienLamViec, CancellationToken cancellationToken = default);
    Task<bool> MergeCartAsync(string maPhienLamViec, int nguoiDungId, CancellationToken cancellationToken = default);
    Task<bool> TransferCartToUserAsync(string maPhienLamViec, int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GioHang>> GetExpiredCartsAsync(int daysOld = 30, CancellationToken cancellationToken = default);
    Task<int> CleanupExpiredCartsAsync(int daysOld = 30, CancellationToken cancellationToken = default);
    Task<bool> ValidateCartItemsAsync(int? nguoiDungId, string? maPhienLamViec, CancellationToken cancellationToken = default);
    Task<IEnumerable<GioHang>> GetInvalidCartItemsAsync(int? nguoiDungId, string? maPhienLamViec, CancellationToken cancellationToken = default);
    Task<bool> ExistsInCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId, CancellationToken cancellationToken = default);
}