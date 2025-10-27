using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;

namespace BilliardShop.Application.Interfaces.Services;

public interface IBaseService<TDto> : IReadOnlyService<TDto> where TDto : BaseDto
{
    Task<ServiceResult<TDto>> CreateAsync(TDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult<TDto>> UpdateAsync(TDto dto, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}