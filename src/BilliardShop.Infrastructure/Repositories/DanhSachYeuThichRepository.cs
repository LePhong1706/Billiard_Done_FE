using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class DanhSachYeuThichRepository : GenericRepository<DanhSachYeuThich>, IDanhSachYeuThichRepository
{
    public DanhSachYeuThichRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DanhSachYeuThich>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaNguoiDung == nguoiDungId && x.SanPham.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<DanhSachYeuThich?> GetByUserAndProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .FirstOrDefaultAsync(x => x.MaNguoiDung == nguoiDungId && x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<bool> IsInWishlistAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(x => x.MaNguoiDung == nguoiDungId && x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<bool> AddToWishlistAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        var exists = await IsInWishlistAsync(nguoiDungId, sanPhamId, cancellationToken);
        if (exists) return true;

        var product = await _context.SanPhams
            .FirstOrDefaultAsync(x => x.Id == sanPhamId && x.TrangThaiHoatDong, cancellationToken);
        
        if (product == null) return false;

        var wishlistItem = new DanhSachYeuThich
        {
            MaNguoiDung = nguoiDungId,
            MaSanPham = sanPhamId,
            NgayTao = DateTime.UtcNow
        };

        await AddAsync(wishlistItem, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveFromWishlistAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        var wishlistItem = await GetByUserAndProductAsync(nguoiDungId, sanPhamId, cancellationToken);
        if (wishlistItem == null) return false;

        Remove(wishlistItem);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> CountByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .CountAsync(x => x.MaNguoiDung == nguoiDungId, cancellationToken);
    }

    public async Task<int> ClearWishlistAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        var wishlistItems = await _dbSet
            .Where(x => x.MaNguoiDung == nguoiDungId)
            .ToListAsync(cancellationToken);

        if (!wishlistItems.Any()) return 0;

        RemoveRange(wishlistItems);
        await _context.SaveChangesAsync(cancellationToken);
        return wishlistItems.Count;
    }

    public async Task<IEnumerable<DanhSachYeuThich>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Where(x => x.MaSanPham == sanPhamId)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByProductAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .CountAsync(x => x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<IEnumerable<DanhSachYeuThich>> GetByCategoryAsync(int nguoiDungId, int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaNguoiDung == nguoiDungId && 
                       x.SanPham.MaDanhMuc == categoryId && 
                       x.SanPham.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhSachYeuThich>> GetByBrandAsync(int nguoiDungId, int brandId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaNguoiDung == nguoiDungId && 
                       x.SanPham.MaThuongHieu == brandId && 
                       x.SanPham.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhSachYeuThich>> GetRecentWishlistAsync(int nguoiDungId, int take = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaNguoiDung == nguoiDungId && x.SanPham.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<(int ProductId, int Count)>> GetPopularWishlistProductsAsync(int top = 10, CancellationToken cancellationToken = default)
    {
        var results = await _dbSet
            .Include(x => x.SanPham)
            .Where(x => x.SanPham.TrangThaiHoatDong)
            .GroupBy(x => x.MaSanPham)
            .Select(g => new { ProductId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(top)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.ProductId, x.Count));
    }

    public async Task<bool> ToggleWishlistAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        var exists = await IsInWishlistAsync(nguoiDungId, sanPhamId, cancellationToken);
        
        if (exists)
        {
            return await RemoveFromWishlistAsync(nguoiDungId, sanPhamId, cancellationToken);
        }
        else
        {
            return await AddToWishlistAsync(nguoiDungId, sanPhamId, cancellationToken);
        }
    }

    public async Task<IEnumerable<DanhSachYeuThich>> GetAvailableWishlistItemsAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaNguoiDung == nguoiDungId && 
                       x.SanPham.TrangThaiHoatDong && 
                       x.SanPham.SoLuongTonKho > 0)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DanhSachYeuThich>> GetOutOfStockWishlistItemsAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaNguoiDung == nguoiDungId && 
                       x.SanPham.TrangThaiHoatDong && 
                       x.SanPham.SoLuongTonKho == 0)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<DanhSachYeuThich> Items, int TotalCount)> GetPagedByUserAsync(
        int nguoiDungId,
        int pageNumber = 1,
        int pageSize = 20,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaNguoiDung == nguoiDungId && x.SanPham.TrangThaiHoatDong);

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "name" => sortDescending 
                ? query.OrderByDescending(x => x.SanPham.TenSanPham)
                : query.OrderBy(x => x.SanPham.TenSanPham),
            "price" => sortDescending
                ? query.OrderByDescending(x => x.SanPham.GiaKhuyenMai ?? x.SanPham.GiaGoc)
                : query.OrderBy(x => x.SanPham.GiaKhuyenMai ?? x.SanPham.GiaGoc),
            "created" => sortDescending
                ? query.OrderByDescending(x => x.NgayTao)
                : query.OrderBy(x => x.NgayTao),
            _ => query.OrderByDescending(x => x.NgayTao)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}