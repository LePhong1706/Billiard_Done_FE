using AutoMapper;
using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;
using BilliardShop.Application.Interfaces.Services;
using BilliardShop.Domain.Common;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Infrastructure.Services;

public abstract class ReadOnlyService<TEntity, TDto> : IReadOnlyService<TDto>
    where TEntity : BaseEntity
    where TDto : BaseDto
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;

    protected ReadOnlyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    protected abstract IGenericRepository<TEntity> Repository { get; }

    public virtual async Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        return entity != null ? _mapper.Map<TDto>(entity) : null;
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TDto>>(entities);
    }

    public virtual async Task<PagedResult<TDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var (entities, totalCount) = await Repository.GetPagedAsync(pageNumber, pageSize);
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);

        return new PagedResult<TDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await Repository.CountAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Repository.AnyAsync(x => x.Id == id, cancellationToken);
    }
}