using AutoMapper;
using BilliardShop.Application.Common.DTOs;
using BilliardShop.Application.Common.Models;
using BilliardShop.Application.Interfaces.Services;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Infrastructure.Services;

public class DanhMucSanPhamService : BaseService<DanhMucSanPham, DanhMucSanPhamDto>, IDanhMucSanPhamService
{
    public DanhMucSanPhamService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    protected override IGenericRepository<DanhMucSanPham> Repository => _unitOfWork.DanhMucSanPhamRepository;

    public async Task<DanhMucSanPhamDto?> GetBySlugAsync(string duongDanDanhMuc, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.DanhMucSanPhamRepository.GetBySlugAsync(duongDanDanhMuc, cancellationToken);
        return category != null ? _mapper.Map<DanhMucSanPhamDto>(category) : null;
    }

    public async Task<IEnumerable<DanhMucSanPhamDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.DanhMucSanPhamRepository.GetActiveAsync(cancellationToken);
        return _mapper.Map<IEnumerable<DanhMucSanPhamDto>>(categories);
    }

    public async Task<IEnumerable<DanhMucSanPhamDto>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.DanhMucSanPhamRepository.GetRootCategoriesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<DanhMucSanPhamDto>>(categories);
    }

    public async Task<IEnumerable<DanhMucSanPhamDto>> GetChildCategoriesAsync(int parentId, CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.DanhMucSanPhamRepository.GetChildCategoriesAsync(parentId, cancellationToken);
        return _mapper.Map<IEnumerable<DanhMucSanPhamDto>>(categories);
    }

    public async Task<IEnumerable<DanhMucSanPhamDto>> GetCategoryHierarchyAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.DanhMucSanPhamRepository.GetCategoryHierarchyAsync(categoryId, cancellationToken);
        return _mapper.Map<IEnumerable<DanhMucSanPhamDto>>(categories);
    }

    public async Task<IEnumerable<DanhMucSanPhamDto>> GetWithProductCountAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.DanhMucSanPhamRepository.GetWithProductCountAsync(cancellationToken);
        return categories.Select(c =>
        {
            var dto = _mapper.Map<DanhMucSanPhamDto>(c);
            dto.SoLuongSanPham = c.SanPhams.Count;
            return dto;
        });
    }

    public async Task<IEnumerable<DanhMucSanPhamDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.DanhMucSanPhamRepository.SearchAsync(searchTerm, cancellationToken);
        return _mapper.Map<IEnumerable<DanhMucSanPhamDto>>(categories);
    }

    public async Task<ServiceResult<bool>> UpdateSortOrderAsync(int categoryId, int sortOrder, CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await _unitOfWork.DanhMucSanPhamRepository.UpdateSortOrderAsync(categoryId, sortOrder, cancellationToken);
            return success ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Failure("Không thể cập nhật thứ tự sắp xếp");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Lỗi khi cập nhật thứ tự: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> UpdateActiveStatusAsync(int categoryId, bool trangThaiHoatDong, CancellationToken cancellationToken = default)
    {
        try
        {
            var category = await Repository.GetByIdAsync(categoryId, cancellationToken);
            if (category == null)
                return ServiceResult<bool>.Failure("Không tìm thấy danh mục");

            category.TrangThaiHoatDong = trangThaiHoatDong;
            Repository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Lỗi khi cập nhật trạng thái: {ex.Message}");
        }
    }

    public async Task<int> CountProductsAsync(int categoryId, bool includeSubcategories = false, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.DanhMucSanPhamRepository.CountProductsAsync(categoryId, includeSubcategories, cancellationToken);
    }

    // Public method to implement interface
    public new async Task<bool> CanDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var hasChildren = await _unitOfWork.DanhMucSanPhamRepository.HasChildrenAsync(id, cancellationToken);
        var hasProducts = await _unitOfWork.DanhMucSanPhamRepository.HasProductsAsync(id, cancellationToken);
        return !hasChildren && !hasProducts;
    }

    public async Task<bool> IsSlugAvailableAsync(string duongDanDanhMuc, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return !await _unitOfWork.DanhMucSanPhamRepository.ExistsBySlugAsync(duongDanDanhMuc, excludeId, cancellationToken);
    }

    protected override async Task<ServiceResult> ValidateForCreateAsync(DanhMucSanPhamDto dto, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (await _unitOfWork.DanhMucSanPhamRepository.ExistsBySlugAsync(dto.DuongDanDanhMuc, null, cancellationToken))
            errors.Add("Đường dẫn danh mục đã tồn tại");

        // Validate parent category exists if specified
        if (dto.MaDanhMucCha.HasValue)
        {
            var parentExists = await Repository.AnyAsync(x => x.Id == dto.MaDanhMucCha.Value, cancellationToken);
            if (!parentExists)
                errors.Add("Danh mục cha không tồn tại");
        }

        return errors.Any() ? ServiceResult.Failure(errors) : ServiceResult.Success();
    }

    protected override async Task<ServiceResult> ValidateForUpdateAsync(DanhMucSanPhamDto dto, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (await _unitOfWork.DanhMucSanPhamRepository.ExistsBySlugAsync(dto.DuongDanDanhMuc, dto.Id, cancellationToken))
            errors.Add("Đường dẫn danh mục đã tồn tại");

        // Validate parent category exists if specified and is not self-referencing
        if (dto.MaDanhMucCha.HasValue)
        {
            if (dto.MaDanhMucCha.Value == dto.Id)
            {
                errors.Add("Danh mục không thể là cha của chính nó");
            }
            else
            {
                var parentExists = await Repository.AnyAsync(x => x.Id == dto.MaDanhMucCha.Value, cancellationToken);
                if (!parentExists)
                    errors.Add("Danh mục cha không tồn tại");
            }
        }

        return errors.Any() ? ServiceResult.Failure(errors) : ServiceResult.Success();
    }
}