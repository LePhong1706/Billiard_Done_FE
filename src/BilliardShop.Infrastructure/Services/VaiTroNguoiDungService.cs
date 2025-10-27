using AutoMapper;
using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;
using BilliardShop.Application.Interfaces.Services;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Infrastructure.Services;

public class VaiTroNguoiDungService : BaseService<VaiTroNguoiDung, VaiTroNguoiDungDto>, IVaiTroNguoiDungService
{
    public VaiTroNguoiDungService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    protected override IGenericRepository<VaiTroNguoiDung> Repository => _unitOfWork.VaiTroNguoiDungRepository;

    public async Task<VaiTroNguoiDungDto?> GetByTenVaiTroAsync(string tenVaiTro, CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.VaiTroNguoiDungRepository.GetByTenVaiTroAsync(tenVaiTro, cancellationToken);
        return role != null ? _mapper.Map<VaiTroNguoiDungDto>(role) : null;
    }

    public async Task<IEnumerable<VaiTroNguoiDungDto>> GetActiveRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.VaiTroNguoiDungRepository.GetActiveRolesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<VaiTroNguoiDungDto>>(roles);
    }

    public async Task<ServiceResult<bool>> UpdateActiveStatusAsync(int vaiTroId, bool trangThaiHoatDong, CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await _unitOfWork.VaiTroNguoiDungRepository.UpdateActiveStatusAsync(vaiTroId, trangThaiHoatDong, cancellationToken);
            return success ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Failure("Không thể cập nhật trạng thái");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Lỗi khi cập nhật trạng thái: {ex.Message}");
        }
    }

    public async Task<IEnumerable<VaiTroNguoiDungDto>> GetRolesWithUserCountAsync(CancellationToken cancellationToken = default)
    {
        var rolesWithCount = await _unitOfWork.VaiTroNguoiDungRepository.GetRolesWithUserCountAsync(cancellationToken);
        return rolesWithCount.Select(x =>
        {
            var dto = _mapper.Map<VaiTroNguoiDungDto>(x.VaiTro);
            dto.SoLuongNguoiDung = x.SoLuongNguoiDung;
            return dto;
        });
    }

    // Public method to implement interface
    public new async Task<bool> CanDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var userCount = await _unitOfWork.VaiTroNguoiDungRepository.CountUsersByRoleAsync(id, cancellationToken);
        return userCount == 0;
    }

    protected override async Task<ServiceResult> ValidateForCreateAsync(VaiTroNguoiDungDto dto, CancellationToken cancellationToken)
    {
        var exists = await _unitOfWork.VaiTroNguoiDungRepository.ExistsByTenVaiTroAsync(dto.TenVaiTro, null, cancellationToken);
        return exists ? ServiceResult.Failure("Tên vai trò đã tồn tại") : ServiceResult.Success();
    }

    protected override async Task<ServiceResult> ValidateForUpdateAsync(VaiTroNguoiDungDto dto, CancellationToken cancellationToken)
    {
        var exists = await _unitOfWork.VaiTroNguoiDungRepository.ExistsByTenVaiTroAsync(dto.TenVaiTro, dto.Id, cancellationToken);
        return exists ? ServiceResult.Failure("Tên vai trò đã tồn tại") : ServiceResult.Success();
    }
}