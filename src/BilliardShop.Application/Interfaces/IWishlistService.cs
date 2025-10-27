using BilliardShop.Domain.Entities;

namespace BilliardShop.Application.Interfaces;

public interface IWishlistService
{
    Task<IEnumerable<DanhSachYeuThich>> GetUserWishlistAsync(int userId);
    Task<bool> AddToWishlistAsync(int userId, int productId);
    Task<bool> RemoveFromWishlistAsync(int userId, int productId);
    Task<bool> IsInWishlistAsync(int userId, int productId);
}
