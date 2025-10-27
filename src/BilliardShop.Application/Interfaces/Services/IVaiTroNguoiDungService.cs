using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;

namespace BilliardShop.Application.Interfaces.Services;

public interface IVaiTroNguoiDungService : IBaseService<VaiTroNguoiDungDto>
{
    Task<VaiTroNguoiDungDto?> GetByTenVaiTroAsync(string tenVaiTro, CancellationToken cancellationToken = default);
    Task<IEnumerable<VaiTroNguoiDungDto>> GetActiveRolesAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdateActiveStatusAsync(int vaiTroId, bool trangThaiHoatDong, CancellationToken cancellationToken = default);
    Task<IEnumerable<VaiTroNguoiDungDto>> GetRolesWithUserCountAsync(CancellationToken cancellationToken = default);
    Task<bool> CanDeleteAsync(int vaiTroId, CancellationToken cancellationToken = default);
}