using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IHinhAnhSanPhamRepository : IGenericRepository<HinhAnhSanPham>
{
    Task<IEnumerable<HinhAnhSanPham>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<HinhAnhSanPham?> GetMainImageAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<HinhAnhSanPham>> GetSortedImagesAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<bool> SetMainImageAsync(int sanPhamId, int hinhAnhId, CancellationToken cancellationToken = default);
    Task<bool> UpdateSortOrderAsync(int hinhAnhId, int sortOrder, CancellationToken cancellationToken = default);
    Task<bool> UpdateMultipleSortOrdersAsync(Dictionary<int, int> imageOrders, CancellationToken cancellationToken = default);
    Task<int> DeleteByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<bool> ExistsForProductAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> CountByProductAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<HinhAnhSanPham>> GetByPathAsync(string imagePath, CancellationToken cancellationToken = default);
    Task<bool> HasMainImageAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<HinhAnhSanPham>> GetImagesWithoutMainAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<HinhAnhSanPham?> GetFirstImageAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<bool> ReorderImagesAsync(int sanPhamId, List<int> imageIds, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetAllImagePathsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<HinhAnhSanPham>> GetOrphanedImagesAsync(CancellationToken cancellationToken = default);
}