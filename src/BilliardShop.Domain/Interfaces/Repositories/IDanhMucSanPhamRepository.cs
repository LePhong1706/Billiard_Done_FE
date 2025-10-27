using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IDanhMucSanPhamRepository : IGenericRepository<DanhMucSanPham>
{
    Task<DanhMucSanPham?> GetBySlugAsync(string duongDanDanhMuc, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPham>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPham>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPham>> GetChildCategoriesAsync(int parentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPham>> GetCategoryHierarchyAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<bool> HasChildrenAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<bool> HasProductsAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySlugAsync(string duongDanDanhMuc, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<int> CountProductsAsync(int categoryId, bool includeSubcategories = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPham>> GetWithProductCountAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPham>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<bool> UpdateSortOrderAsync(int categoryId, int sortOrder, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPham>> GetByParentIdAsync(int? parentId, CancellationToken cancellationToken = default);
}