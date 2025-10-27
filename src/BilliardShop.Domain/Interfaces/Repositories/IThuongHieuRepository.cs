using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IThuongHieuRepository : IGenericRepository<ThuongHieu>
{
    Task<ThuongHieu?> GetBySlugAsync(string duongDanThuongHieu, CancellationToken cancellationToken = default);
    Task<ThuongHieu?> GetByNameAsync(string tenThuongHieu, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieu>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string tenThuongHieu, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySlugAsync(string duongDanThuongHieu, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<int> CountProductsAsync(int thuongHieuId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieu>> GetWithProductCountAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieu>> GetByCountryAsync(string quocGia, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieu>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieu>> GetPopularBrandsAsync(int top = 10, CancellationToken cancellationToken = default);
    Task<bool> HasProductsAsync(int thuongHieuId, CancellationToken cancellationToken = default);
}