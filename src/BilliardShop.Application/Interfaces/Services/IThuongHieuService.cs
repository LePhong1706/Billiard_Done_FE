using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;

namespace BilliardShop.Application.Interfaces.Services;

public interface IThuongHieuService : IBaseService<ThuongHieuDto>
{
    Task<ThuongHieuDto?> GetBySlugAsync(string duongDanThuongHieu, CancellationToken cancellationToken = default);
    Task<ThuongHieuDto?> GetByNameAsync(string tenThuongHieu, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieuDto>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieuDto>> GetWithProductCountAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieuDto>> GetByCountryAsync(string quocGia, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieuDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<ThuongHieuDto>> GetPopularBrandsAsync(int top = 10, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdateActiveStatusAsync(int thuongHieuId, bool trangThaiHoatDong, CancellationToken cancellationToken = default);
    Task<bool> CanDeleteAsync(int thuongHieuId, CancellationToken cancellationToken = default);
    Task<bool> IsSlugAvailableAsync(string duongDanThuongHieu, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> IsNameAvailableAsync(string tenThuongHieu, int? excludeId = null, CancellationToken cancellationToken = default);
}