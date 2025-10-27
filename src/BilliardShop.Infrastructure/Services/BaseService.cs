using AutoMapper;
using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;
using BilliardShop.Application.Interfaces.Services;
using BilliardShop.Domain.Common;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Infrastructure.Services;

public abstract class BaseService<TEntity, TDto> : ReadOnlyService<TEntity, TDto>, IBaseService<TDto>
    where TEntity : BaseEntity
    where TDto : BaseDto
{
    protected BaseService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public virtual async Task<ServiceResult<TDto>> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await ValidateForCreateAsync(dto, cancellationToken);
            if (!validationResult.IsSuccess)
                return ServiceResult<TDto>.Failure(validationResult.Errors);

            var entity = _mapper.Map<TEntity>(dto);
            var createdEntity = await Repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createdDto = _mapper.Map<TDto>(createdEntity);
            return ServiceResult<TDto>.Success(createdDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<TDto>.Failure($"Lỗi khi tạo: {ex.Message}");
        }
    }

    public virtual async Task<ServiceResult<TDto>> UpdateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingEntity = await Repository.GetByIdAsync(dto.Id, cancellationToken);
            if (existingEntity == null)
                return ServiceResult<TDto>.Failure("Không tìm thấy bản ghi");

            var validationResult = await ValidateForUpdateAsync(dto, cancellationToken);
            if (!validationResult.IsSuccess)
                return ServiceResult<TDto>.Failure(validationResult.Errors);

            _mapper.Map(dto, existingEntity);
            Repository.Update(existingEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedDto = _mapper.Map<TDto>(existingEntity);
            return ServiceResult<TDto>.Success(updatedDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<TDto>.Failure($"Lỗi khi cập nhật: {ex.Message}");
        }
    }

    public virtual async Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await Repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return ServiceResult.Failure("Không tìm thấy bản ghi");

            var canDelete = await CanDeleteAsync(id, cancellationToken);
            if (!canDelete)
                return ServiceResult.Failure("Không thể xóa bản ghi này do có dữ liệu liên quan");

            Repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failure($"Lỗi khi xóa: {ex.Message}");
        }
    }

    public virtual async Task<ServiceResult> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = new List<TEntity>();
            foreach (var id in ids)
            {
                var entity = await Repository.GetByIdAsync(id, cancellationToken);
                if (entity == null)
                    return ServiceResult.Failure($"Không tìm thấy bản ghi với ID: {id}");

                var canDelete = await CanDeleteAsync(id, cancellationToken);
                if (!canDelete)
                    return ServiceResult.Failure($"Không thể xóa bản ghi ID {id} do có dữ liệu liên quan");

                entities.Add(entity);
            }

            Repository.RemoveRange(entities);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failure($"Lỗi khi xóa hàng loạt: {ex.Message}");
        }
    }

    protected virtual Task<ServiceResult> ValidateForCreateAsync(TDto dto, CancellationToken cancellationToken)
    {
        return Task.FromResult(ServiceResult.Success());
    }

    protected virtual Task<ServiceResult> ValidateForUpdateAsync(TDto dto, CancellationToken cancellationToken)
    {
        return Task.FromResult(ServiceResult.Success());
    }

    protected virtual Task<bool> CanDeleteAsync(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}