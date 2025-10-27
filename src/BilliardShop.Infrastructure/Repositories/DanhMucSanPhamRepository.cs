using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class DanhMucSanPhamRepository : GenericRepository<DanhMucSanPham>, IDanhMucSanPhamRepository
{
    public DanhMucSanPhamRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<DanhMucSanPham?> GetBySlugAsync(string duongDanDanhMuc, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DanhMucCha)
            .Include(x => x.DanhMucCons)
            .FirstOrDefaultAsync(x => x.DuongDanDanhMuc == duongDanDanhMuc, cancellationToken);
    }

    public async Task<IEnumerable<DanhMucSanPham>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.TenDanhMuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhMucSanPham>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaDanhMucCha == null && x.TrangThaiHoatDong)
            .Include(x => x.DanhMucCons.Where(c => c.TrangThaiHoatDong))
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.TenDanhMuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhMucSanPham>> GetChildCategoriesAsync(int parentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaDanhMucCha == parentId && x.TrangThaiHoatDong)
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.TenDanhMuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhMucSanPham>> GetCategoryHierarchyAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var categories = new List<DanhMucSanPham>();
        var currentCategory = await _dbSet
            .Include(x => x.DanhMucCha)
            .FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

        while (currentCategory != null)
        {
            categories.Insert(0, currentCategory);
            currentCategory = currentCategory.DanhMucCha;
        }

        return categories;
    }

    public async Task<bool> HasChildrenAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(x => x.MaDanhMucCha == categoryId, cancellationToken);
    }

    public async Task<bool> HasProductsAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.SanPhams
            .AnyAsync(x => x.MaDanhMuc == categoryId, cancellationToken);
    }

    public async Task<bool> ExistsBySlugAsync(string duongDanDanhMuc, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.DuongDanDanhMuc == duongDanDanhMuc);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<int> CountProductsAsync(int categoryId, bool includeSubcategories = false, CancellationToken cancellationToken = default)
    {
        if (!includeSubcategories)
        {
            return await _context.SanPhams
                .CountAsync(x => x.MaDanhMuc == categoryId && x.TrangThaiHoatDong, cancellationToken);
        }

        var categoryIds = new List<int> { categoryId };
        await GetAllSubcategoryIds(categoryId, categoryIds, cancellationToken);

        return await _context.SanPhams
            .CountAsync(x => categoryIds.Contains(x.MaDanhMuc) && x.TrangThaiHoatDong, cancellationToken);
    }

    public async Task<IEnumerable<DanhMucSanPham>> GetWithProductCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .Select(x => new DanhMucSanPham
            {
                Id = x.Id,
                TenDanhMuc = x.TenDanhMuc,
                DuongDanDanhMuc = x.DuongDanDanhMuc,
                MoTa = x.MoTa,
                MaDanhMucCha = x.MaDanhMucCha,
                HinhAnhDaiDien = x.HinhAnhDaiDien,
                ThuTuSapXep = x.ThuTuSapXep,
                TrangThaiHoatDong = x.TrangThaiHoatDong,
                NgayTao = x.NgayTao,
                SanPhams = x.SanPhams.Where(p => p.TrangThaiHoatDong).ToList()
            })
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.TenDanhMuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhMucSanPham>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var searchLower = searchTerm.ToLower();
        return await _dbSet
            .Where(x => x.TenDanhMuc.ToLower().Contains(searchLower) ||
                       (x.MoTa != null && x.MoTa.ToLower().Contains(searchLower)))
            .OrderBy(x => x.TenDanhMuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateSortOrderAsync(int categoryId, int sortOrder, CancellationToken cancellationToken = default)
    {
        var category = await GetByIdAsync(categoryId, cancellationToken);
        if (category == null)
            return false;

        category.ThuTuSapXep = sortOrder;
        Update(category);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<DanhMucSanPham>> GetByParentIdAsync(int? parentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaDanhMucCha == parentId && x.TrangThaiHoatDong)
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.TenDanhMuc)
            .ToListAsync(cancellationToken);
    }

    private async Task GetAllSubcategoryIds(int parentId, List<int> categoryIds, CancellationToken cancellationToken)
    {
        var subcategories = await _dbSet
            .Where(x => x.MaDanhMucCha == parentId)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var subcategoryId in subcategories)
        {
            if (!categoryIds.Contains(subcategoryId))
            {
                categoryIds.Add(subcategoryId);
                await GetAllSubcategoryIds(subcategoryId, categoryIds, cancellationToken);
            }
        }
    }
}