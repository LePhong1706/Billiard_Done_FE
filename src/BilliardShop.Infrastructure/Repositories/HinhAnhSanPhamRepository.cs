using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class HinhAnhSanPhamRepository : GenericRepository<HinhAnhSanPham>, IHinhAnhSanPhamRepository
{
    public HinhAnhSanPhamRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<HinhAnhSanPham>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaSanPham == sanPhamId)
            .OrderByDescending(x => x.LaHinhAnhChinh)
            .ThenBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<HinhAnhSanPham?> GetMainImageAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.MaSanPham == sanPhamId && x.LaHinhAnhChinh, cancellationToken);
    }

    public async Task<IEnumerable<HinhAnhSanPham>> GetSortedImagesAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaSanPham == sanPhamId)
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> SetMainImageAsync(int sanPhamId, int hinhAnhId, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var currentMainImages = await _dbSet
                .Where(x => x.MaSanPham == sanPhamId && x.LaHinhAnhChinh)
                .ToListAsync(cancellationToken);

            foreach (var img in currentMainImages)
            {
                img.LaHinhAnhChinh = false;
                Update(img);
            }

            var newMainImage = await _dbSet
                .FirstOrDefaultAsync(x => x.Id == hinhAnhId && x.MaSanPham == sanPhamId, cancellationToken);

            if (newMainImage == null)
                return false;

            newMainImage.LaHinhAnhChinh = true;
            Update(newMainImage);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return true;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
    }

    public async Task<bool> UpdateSortOrderAsync(int hinhAnhId, int sortOrder, CancellationToken cancellationToken = default)
    {
        var image = await GetByIdAsync(hinhAnhId, cancellationToken);
        if (image == null) return false;

        image.ThuTuSapXep = sortOrder;
        Update(image);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateMultipleSortOrdersAsync(Dictionary<int, int> imageOrders, CancellationToken cancellationToken = default)
    {
        var imageIds = imageOrders.Keys.ToList();
        var images = await _dbSet
            .Where(x => imageIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var image in images)
        {
            if (imageOrders.TryGetValue(image.Id, out var sortOrder))
            {
                image.ThuTuSapXep = sortOrder;
                Update(image);
            }
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> DeleteByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        var images = await _dbSet
            .Where(x => x.MaSanPham == sanPhamId)
            .ToListAsync(cancellationToken);

        if (!images.Any()) return 0;

        RemoveRange(images);
        await _context.SaveChangesAsync(cancellationToken);
        return images.Count;
    }

    public async Task<bool> ExistsForProductAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<int> CountByProductAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<IEnumerable<HinhAnhSanPham>> GetByPathAsync(string imagePath, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .Where(x => x.DuongDanHinhAnh == imagePath)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasMainImageAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.MaSanPham == sanPhamId && x.LaHinhAnhChinh, cancellationToken);
    }

    public async Task<IEnumerable<HinhAnhSanPham>> GetImagesWithoutMainAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaSanPham == sanPhamId && !x.LaHinhAnhChinh)
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<HinhAnhSanPham?> GetFirstImageAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaSanPham == sanPhamId)
            .OrderByDescending(x => x.LaHinhAnhChinh)
            .ThenBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.NgayTao)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ReorderImagesAsync(int sanPhamId, List<int> imageIds, CancellationToken cancellationToken = default)
    {
        var images = await _dbSet
            .Where(x => x.MaSanPham == sanPhamId && imageIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        for (int i = 0; i < imageIds.Count; i++)
        {
            var image = images.FirstOrDefault(x => x.Id == imageIds[i]);
            if (image != null)
            {
                image.ThuTuSapXep = i + 1;
                Update(image);
            }
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<string>> GetAllImagePathsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Select(x => x.DuongDanHinhAnh)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<HinhAnhSanPham>> GetOrphanedImagesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => !_context.SanPhams.Any(p => p.Id == x.MaSanPham))
            .ToListAsync(cancellationToken);
    }
}