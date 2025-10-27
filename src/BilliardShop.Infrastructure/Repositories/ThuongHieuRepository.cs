using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class ThuongHieuRepository : GenericRepository<ThuongHieu>, IThuongHieuRepository
{
    public ThuongHieuRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<ThuongHieu?> GetBySlugAsync(string duongDanThuongHieu, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.DuongDanThuongHieu == duongDanThuongHieu, cancellationToken);
    }

    public async Task<ThuongHieu?> GetByNameAsync(string tenThuongHieu, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.TenThuongHieu == tenThuongHieu, cancellationToken);
    }

    public async Task<IEnumerable<ThuongHieu>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .OrderBy(x => x.TenThuongHieu)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string tenThuongHieu, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TenThuongHieu == tenThuongHieu);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsBySlugAsync(string duongDanThuongHieu, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.DuongDanThuongHieu == duongDanThuongHieu);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<int> CountProductsAsync(int thuongHieuId, CancellationToken cancellationToken = default)
    {
        return await _context.SanPhams
            .CountAsync(x => x.MaThuongHieu == thuongHieuId && x.TrangThaiHoatDong, cancellationToken);
    }

    public async Task<IEnumerable<ThuongHieu>> GetWithProductCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .Select(x => new ThuongHieu
            {
                Id = x.Id,
                TenThuongHieu = x.TenThuongHieu,
                DuongDanThuongHieu = x.DuongDanThuongHieu,
                MoTa = x.MoTa,
                LogoThuongHieu = x.LogoThuongHieu,
                Website = x.Website,
                QuocGia = x.QuocGia,
                TrangThaiHoatDong = x.TrangThaiHoatDong,
                NgayTao = x.NgayTao,
                SanPhams = x.SanPhams.Where(p => p.TrangThaiHoatDong).ToList()
            })
            .OrderBy(x => x.TenThuongHieu)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ThuongHieu>> GetByCountryAsync(string quocGia, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.QuocGia == quocGia && x.TrangThaiHoatDong)
            .OrderBy(x => x.TenThuongHieu)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ThuongHieu>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var searchLower = searchTerm.ToLower();
        return await _dbSet
            .Where(x => x.TenThuongHieu.ToLower().Contains(searchLower) ||
                       (x.MoTa != null && x.MoTa.ToLower().Contains(searchLower)) ||
                       (x.QuocGia != null && x.QuocGia.ToLower().Contains(searchLower)))
            .OrderBy(x => x.TenThuongHieu)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ThuongHieu>> GetPopularBrandsAsync(int top = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .GroupJoin(
                _context.SanPhams.Where(p => p.TrangThaiHoatDong),
                brand => brand.Id,
                product => product.MaThuongHieu,
                (brand, products) => new { Brand = brand, ProductCount = products.Count() }
            )
            .OrderByDescending(x => x.ProductCount)
            .Take(top)
            .Select(x => x.Brand)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasProductsAsync(int thuongHieuId, CancellationToken cancellationToken = default)
    {
        return await _context.SanPhams
            .AnyAsync(x => x.MaThuongHieu == thuongHieuId, cancellationToken);
    }
}