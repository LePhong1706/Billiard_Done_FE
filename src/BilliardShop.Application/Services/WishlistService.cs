using BilliardShop.Application.Interfaces;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Application.Services;

public class WishlistService : IWishlistService
{
    private readonly IUnitOfWork _unitOfWork;

    public WishlistService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<DanhSachYeuThich>> GetUserWishlistAsync(int userId)
    {
        var wishlist = await _unitOfWork.DanhSachYeuThichRepository
            .FindAsync(
                w => w.MaNguoiDung == userId,
                w => w.SanPham,
                w => w.SanPham.HinhAnhs
            );

        return wishlist.OrderByDescending(w => w.NgayTao);
    }

    public async Task<bool> AddToWishlistAsync(int userId, int productId)
    {
        // Check if already in wishlist
        if (await IsInWishlistAsync(userId, productId))
            return false;

        var wishlistItem = new DanhSachYeuThich
        {
            MaNguoiDung = userId,
            MaSanPham = productId,
            NgayTao = DateTime.UtcNow
        };

        await _unitOfWork.DanhSachYeuThichRepository.AddAsync(wishlistItem);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveFromWishlistAsync(int userId, int productId)
    {
        var wishlistItems = await _unitOfWork.DanhSachYeuThichRepository
            .FindAsync(w => w.MaNguoiDung == userId && w.MaSanPham == productId);

        var item = wishlistItems.FirstOrDefault();
        if (item == null)
            return false;

        _unitOfWork.DanhSachYeuThichRepository.Remove(item);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IsInWishlistAsync(int userId, int productId)
    {
        var count = await _unitOfWork.DanhSachYeuThichRepository
            .CountAsync(w => w.MaNguoiDung == userId && w.MaSanPham == productId);

        return count > 0;
    }
}
