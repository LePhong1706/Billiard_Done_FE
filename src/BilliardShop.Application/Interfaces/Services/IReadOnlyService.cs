using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;

namespace BilliardShop.Application.Interfaces.Services;

public interface IReadOnlyService<TDto> where TDto : BaseDto
{
    Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<TDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}