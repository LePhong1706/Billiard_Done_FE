using AutoMapper;
using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;
using BilliardShop.Application.Interfaces.Services;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Infrastructure.Services;

public class ThuongHieuService : BaseService<ThuongHieu, ThuongHieuDto>, IThuongHieuService
{
    public ThuongHieuService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    protected override IGenericRepository<ThuongHieu> Repository => _unitOfWork.ThuongHieuRepository;

    public async Task<ThuongHieuDto?> GetBySlugAsync(string duongDanThuongHieu, CancellationToken cancellationToken = default)
    {
        var brand = await _unitOfWork.ThuongHieuRepository.GetBySlugAsync(duongDanThuongHieu, cancellationToken);
        return brand != null ? _mapper.Map<ThuongHieuDto>(brand) : null;
    }

    public async Task<ThuongHieuDto?> GetByNameAsync(string tenThuongHieu, CancellationToken cancellationToken = default)
    {
        var brand = await _unitOfWork.ThuongHieuRepository.GetByNameAsync(tenThuongHieu, cancellationToken);
        return brand != null ? _mapper.Map<ThuongHieuDto>(brand) : null;
    }

    public async Task<IEnumerable<ThuongHieuDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var brands = await _unitOfWork.ThuongHieuRepository.GetActiveAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ThuongHieuDto>>(brands);
    }

    public async Task<IEnumerable<ThuongHieuDto>> GetWithProductCountAsync(CancellationToken cancellationToken = default)
    {
        var brands = await _unitOfWork.ThuongHieuRepository.GetWithProductCountAsync(cancellationToken);
        return brands.Select(b =>
        {
            var dto = _mapper.Map<ThuongHieuDto>(b);
            dto.SoLuongSanPham = b.SanPhams.Count;
            return dto;
        });
    }

    public async Task<IEnumerable<ThuongHieuDto>> GetByCountryAsync(string quocGia, CancellationToken cancellationToken = default)
    {
        var brands = await _unitOfWork.ThuongHieuRepository.GetByCountryAsync(quocGia, cancellationToken);
        return _mapper.Map<IEnumerable<ThuongHieuDto>>(brands);
    }

    public async Task<IEnumerable<ThuongHieuDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var brands = await _unitOfWork.ThuongHieuRepository.SearchAsync(searchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<ThuongHieuDto>>(brands);
    }

    public async Task<IEnumerable<ThuongHieuDto>> GetPopularBrandsAsync(int top = 10, CancellationToken cancellationToken = default)
    {
        var brands = await _unitOfWork.ThuongHieuRepository.GetPopularBrandsAsync(top, cancellationToken);
        return _mapper.Map<IEnumerable<ThuongHieuDto>>(brands);
    }

    public async Task<ServiceResult<bool>> UpdateActiveStatusAsync(int thuongHieuId, bool trangThaiHoatDong, CancellationToken cancellationToken = default)
    {
        try
        {
            var brand = await Repository.GetByIdAsync(thuongHieuId, cancellationToken);
            if (brand == null)
                return ServiceResult<bool>.Failure("Không tìm thấy thương hiệu");

            brand.TrangThaiHoatDong = trangThaiHoatDong;
            Repository.Update(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Lỗi khi cập nhật trạng thái: {ex.Message}");
        }
    }

    // Public method to implement interface
    public new async Task<bool> CanDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return !await _unitOfWork.ThuongHieuRepository.HasProductsAsync(id, cancellationToken);
    }

    public async Task<bool> IsSlugAvailableAsync(string duongDanThuongHieu, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return !await _unitOfWork.ThuongHieuRepository.ExistsBySlugAsync(duongDanThuongHieu, excludeId, cancellationToken);
    }

    public async Task<bool> IsNameAvailableAsync(string tenThuongHieu, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return !await _unitOfWork.ThuongHieuRepository.ExistsByNameAsync(tenThuongHieu, excludeId, cancellationToken);
    }

    protected override async Task<ServiceResult> ValidateForCreateAsync(ThuongHieuDto dto, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (await _unitOfWork.ThuongHieuRepository.ExistsByNameAsync(dto.TenThuongHieu, null, cancellationToken))
            errors.Add("Tên thương hiệu đã tồn tại");

        if (await _unitOfWork.ThuongHieuRepository.ExistsBySlugAsync(dto.DuongDanThuongHieu, null, cancellationToken))
            errors.Add("Đường dẫn thương hiệu đã tồn tại");

        return errors.Any() ? ServiceResult.Failure(errors) : ServiceResult.Success();
    }

    protected override async Task<ServiceResult> ValidateForUpdateAsync(ThuongHieuDto dto, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (await _unitOfWork.ThuongHieuRepository.ExistsByNameAsync(dto.TenThuongHieu, dto.Id, cancellationToken))
            errors.Add("Tên thương hiệu đã tồn tại");

        if (await _unitOfWork.ThuongHieuRepository.ExistsBySlugAsync(dto.DuongDanThuongHieu, dto.Id, cancellationToken))
            errors.Add("Đường dẫn thương hiệu đã tồn tại");

        return errors.Any() ? ServiceResult.Failure(errors) : ServiceResult.Success();
    }
}