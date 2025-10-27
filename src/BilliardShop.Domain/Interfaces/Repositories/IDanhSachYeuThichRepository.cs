using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IDanhSachYeuThichRepository : IGenericRepository<DanhSachYeuThich>
{
    Task<IEnumerable<DanhSachYeuThich>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<DanhSachYeuThich?> GetByUserAndProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<bool> IsInWishlistAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<bool> AddToWishlistAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<bool> RemoveFromWishlistAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> CountByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<int> ClearWishlistAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhSachYeuThich>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> CountByProductAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhSachYeuThich>> GetByCategoryAsync(int nguoiDungId, int categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhSachYeuThich>> GetByBrandAsync(int nguoiDungId, int brandId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhSachYeuThich>> GetRecentWishlistAsync(int nguoiDungId, int take = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int ProductId, int Count)>> GetPopularWishlistProductsAsync(int top = 10, CancellationToken cancellationToken = default);
    Task<bool> ToggleWishlistAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhSachYeuThich>> GetAvailableWishlistItemsAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhSachYeuThich>> GetOutOfStockWishlistItemsAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<DanhSachYeuThich> Items, int TotalCount)> GetPagedByUserAsync(
        int nguoiDungId,
        int pageNumber = 1,
        int pageSize = 20,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default);
}