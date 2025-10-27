using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;

namespace BilliardShop.Application.Interfaces.Services;

public interface IDanhMucSanPhamService : IBaseService<DanhMucSanPhamDto>
{
    Task<DanhMucSanPhamDto?> GetBySlugAsync(string duongDanDanhMuc, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPhamDto>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPhamDto>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPhamDto>> GetChildCategoriesAsync(int parentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPhamDto>> GetCategoryHierarchyAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPhamDto>> GetWithProductCountAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DanhMucSanPhamDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdateSortOrderAsync(int categoryId, int sortOrder, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdateActiveStatusAsync(int categoryId, bool trangThaiHoatDong, CancellationToken cancellationToken = default);
    Task<int> CountProductsAsync(int categoryId, bool includeSubcategories = false, CancellationToken cancellationToken = default);
    Task<bool> CanDeleteAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<bool> IsSlugAvailableAsync(string duongDanDanhMuc, int? excludeId = null, CancellationToken cancellationToken = default);
}