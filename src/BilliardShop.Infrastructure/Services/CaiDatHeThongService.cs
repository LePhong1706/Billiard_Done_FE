using AutoMapper;
using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;
using BilliardShop.Application.Interfaces.Services;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Infrastructure.Services;

public class CaiDatHeThongService : ReadOnlyService<CaiDatHeThong, CaiDatHeThongDto>, ICaiDatHeThongService
{
    public CaiDatHeThongService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    protected override IGenericRepository<CaiDatHeThong> Repository => _unitOfWork.CaiDatHeThongRepository;

    public async Task<ServiceResult<CaiDatHeThongDto>> CreateOrUpdateAsync(CaiDatHeThongDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await _unitOfWork.CaiDatHeThongRepository.SetValueAsync(
                dto.KhoaCaiDat, dto.GiaTriCaiDat, dto.MoTa, dto.KieuDuLieu, dto.NguoiCapNhatCuoi, cancellationToken);

            if (!success)
                return ServiceResult<CaiDatHeThongDto>.Failure("Không thể tạo hoặc cập nhật cài đặt");

            var updatedSetting = await _unitOfWork.CaiDatHeThongRepository.GetByKeyAsync(dto.KhoaCaiDat, cancellationToken);
            var updatedDto = _mapper.Map<CaiDatHeThongDto>(updatedSetting);

            return ServiceResult<CaiDatHeThongDto>.Success(updatedDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<CaiDatHeThongDto>.Failure($"Lỗi khi tạo/cập nhật cài đặt: {ex.Message}");
        }
    }

    public async Task<ServiceResult> DeleteByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await _unitOfWork.CaiDatHeThongRepository.DeleteByKeyAsync(khoaCaiDat, cancellationToken);
            return success ? ServiceResult.Success() : ServiceResult.Failure("Không tìm thấy cài đặt để xóa");
        }
        catch (Exception ex)
        {
            return ServiceResult.Failure($"Lỗi khi xóa cài đặt: {ex.Message}");
        }
    }

    public async Task<CaiDatHeThongDto?> GetByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default)
    {
        var setting = await _unitOfWork.CaiDatHeThongRepository.GetByKeyAsync(khoaCaiDat, cancellationToken);
        return setting != null ? _mapper.Map<CaiDatHeThongDto>(setting) : null;
    }

    public async Task<string?> GetValueAsync(string khoaCaiDat, string? defaultValue = null, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.CaiDatHeThongRepository.GetValueAsync(khoaCaiDat, defaultValue, cancellationToken);
    }

    public async Task<int> GetIntValueAsync(string khoaCaiDat, int defaultValue = 0, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.CaiDatHeThongRepository.GetIntValueAsync(khoaCaiDat, defaultValue, cancellationToken);
    }

    public async Task<decimal> GetDecimalValueAsync(string khoaCaiDat, decimal defaultValue = 0, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.CaiDatHeThongRepository.GetDecimalValueAsync(khoaCaiDat, defaultValue, cancellationToken);
    }

    public async Task<bool> GetBoolValueAsync(string khoaCaiDat, bool defaultValue = false, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.CaiDatHeThongRepository.GetBoolValueAsync(khoaCaiDat, defaultValue, cancellationToken);
    }

    public async Task<ServiceResult> SetValueAsync(string khoaCaiDat, string? giaTriCaiDat, string? moTa = null, string kieuDuLieu = "String", int? nguoiCapNhat = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await _unitOfWork.CaiDatHeThongRepository.SetValueAsync(khoaCaiDat, giaTriCaiDat, moTa, kieuDuLieu, nguoiCapNhat, cancellationToken);
            return success ? ServiceResult.Success() : ServiceResult.Failure("Không thể cập nhật cài đặt");
        }
        catch (Exception ex)
        {
            return ServiceResult.Failure($"Lỗi khi cập nhật cài đặt: {ex.Message}");
        }
    }

    public async Task<ServiceResult<int>> SetMultipleValuesAsync(Dictionary<string, string?> caiDats, int? nguoiCapNhat = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var updateCount = await _unitOfWork.CaiDatHeThongRepository.SetMultipleValuesAsync(caiDats, nguoiCapNhat, cancellationToken);
            return ServiceResult<int>.Success(updateCount);
        }
        catch (Exception ex)
        {
            return ServiceResult<int>.Failure($"Lỗi khi cập nhật nhiều cài đặt: {ex.Message}");
        }
    }

    public async Task<IEnumerable<CaiDatHeThongDto>> GetByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        var settings = await _unitOfWork.CaiDatHeThongRepository.GetByPrefixAsync(prefix, cancellationToken);
        return _mapper.Map<IEnumerable<CaiDatHeThongDto>>(settings);
    }

    public async Task<IEnumerable<CaiDatHeThongDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var settings = await _unitOfWork.CaiDatHeThongRepository.SearchAsync(searchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<CaiDatHeThongDto>>(settings);
    }

    public async Task<Dictionary<string, string?>> GetAllAsDictionaryAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.CaiDatHeThongRepository.GetAllAsDictionaryAsync(cancellationToken);
    }
}