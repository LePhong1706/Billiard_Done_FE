using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class ThuocTinhSanPhamRepository : GenericRepository<ThuocTinhSanPham>, IThuocTinhSanPhamRepository
{
    public ThuocTinhSanPhamRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ThuocTinhSanPham>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaSanPham == sanPhamId)
            .OrderBy(x => x.TenThuocTinh)
            .ToListAsync(cancellationToken);
    }

    public async Task<ThuocTinhSanPham?> GetByProductAndNameAsync(int sanPhamId, string tenThuocTinh, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.MaSanPham == sanPhamId && x.TenThuocTinh == tenThuocTinh, cancellationToken);
    }

    public async Task<IEnumerable<ThuocTinhSanPham>> GetByAttributeNameAsync(string tenThuocTinh, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .Where(x => x.TenThuocTinh == tenThuocTinh)
            .OrderBy(x => x.GiaTriThuocTinh)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>> GetUniqueAttributeNamesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Select(x => x.TenThuocTinh)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>> GetUniqueAttributeValuesAsync(string tenThuocTinh, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TenThuocTinh == tenThuocTinh && x.GiaTriThuocTinh != null)
            .Select(x => x.GiaTriThuocTinh!)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<(string TenThuocTinh, IEnumerable<string> GiaTriThuocTinhs)>> GetAttributeOptionsAsync(CancellationToken cancellationToken = default)
    {
        var attributes = await _dbSet
            .Where(x => x.GiaTriThuocTinh != null)
            .GroupBy(x => x.TenThuocTinh)
            .Select(g => new
            {
                TenThuocTinh = g.Key,
                GiaTriThuocTinhs = g.Select(x => x.GiaTriThuocTinh!).Distinct().OrderBy(v => v)
            })
            .OrderBy(x => x.TenThuocTinh)
            .ToListAsync(cancellationToken);

        return attributes.Select(x => (x.TenThuocTinh, x.GiaTriThuocTinhs.AsEnumerable()));
    }

    public async Task<bool> UpdateAttributeValueAsync(int sanPhamId, string tenThuocTinh, string? giaTriThuocTinh, CancellationToken cancellationToken = default)
    {
        var existing = await GetByProductAndNameAsync(sanPhamId, tenThuocTinh, cancellationToken);

        if (existing != null)
        {
            if (string.IsNullOrWhiteSpace(giaTriThuocTinh))
            {
                Remove(existing);
            }
            else
            {
                existing.GiaTriThuocTinh = giaTriThuocTinh;
                Update(existing);
            }
        }
        else if (!string.IsNullOrWhiteSpace(giaTriThuocTinh))
        {
            var newAttribute = new ThuocTinhSanPham
            {
                MaSanPham = sanPhamId,
                TenThuocTinh = tenThuocTinh,
                GiaTriThuocTinh = giaTriThuocTinh
            };
            await AddAsync(newAttribute, cancellationToken);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> SetMultipleAttributesAsync(int sanPhamId, Dictionary<string, string?> attributes, CancellationToken cancellationToken = default)
    {
        var existingAttributes = await GetByProductIdAsync(sanPhamId, cancellationToken);
        var existingDict = existingAttributes.ToDictionary(x => x.TenThuocTinh, x => x);

        foreach (var kvp in attributes)
        {
            if (existingDict.TryGetValue(kvp.Key, out var existing))
            {
                if (string.IsNullOrWhiteSpace(kvp.Value))
                {
                    Remove(existing);
                }
                else
                {
                    existing.GiaTriThuocTinh = kvp.Value;
                    Update(existing);
                }
                existingDict.Remove(kvp.Key);
            }
            else if (!string.IsNullOrWhiteSpace(kvp.Value))
            {
                var newAttribute = new ThuocTinhSanPham
                {
                    MaSanPham = sanPhamId,
                    TenThuocTinh = kvp.Key,
                    GiaTriThuocTinh = kvp.Value
                };
                await AddAsync(newAttribute, cancellationToken);
            }
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> DeleteByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        var attributes = await _dbSet
            .Where(x => x.MaSanPham == sanPhamId)
            .ToListAsync(cancellationToken);

        if (!attributes.Any()) return 0;

        RemoveRange(attributes);
        await _context.SaveChangesAsync(cancellationToken);
        return attributes.Count;
    }

    public async Task<int> DeleteByAttributeNameAsync(string tenThuocTinh, CancellationToken cancellationToken = default)
    {
        var attributes = await _dbSet
            .Where(x => x.TenThuocTinh == tenThuocTinh)
            .ToListAsync(cancellationToken);

        if (!attributes.Any()) return 0;

        RemoveRange(attributes);
        await _context.SaveChangesAsync(cancellationToken);
        return attributes.Count;
    }

    public async Task<bool> ExistsForProductAsync(int sanPhamId, string tenThuocTinh, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(x => x.MaSanPham == sanPhamId && x.TenThuocTinh == tenThuocTinh, cancellationToken);
    }

    public async Task<int> CountByProductAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<int> CountByAttributeNameAsync(string tenThuocTinh, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.TenThuocTinh == tenThuocTinh, cancellationToken);
    }

    public async Task<IEnumerable<ThuocTinhSanPham>> SearchByValueAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var searchLower = searchTerm.ToLower();
        return await _dbSet
            .Include(x => x.SanPham)
            .Where(x => x.TenThuocTinh.ToLower().Contains(searchLower) ||
                       (x.GiaTriThuocTinh != null && x.GiaTriThuocTinh.ToLower().Contains(searchLower)))
            .OrderBy(x => x.TenThuocTinh)
            .ThenBy(x => x.GiaTriThuocTinh)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ThuocTinhSanPham>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .Where(x => x.SanPham.MaDanhMuc == categoryId && x.SanPham.TrangThaiHoatDong)
            .OrderBy(x => x.TenThuocTinh)
            .ThenBy(x => x.GiaTriThuocTinh)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<string, string?>> GetAttributesDictionaryAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        var attributes = await GetByProductIdAsync(sanPhamId, cancellationToken);
        return attributes.ToDictionary(x => x.TenThuocTinh, x => x.GiaTriThuocTinh);
    }

    public async Task<bool> BulkUpdateAttributesAsync(int sanPhamId, Dictionary<string, string?> newAttributes, bool deleteOthers = false, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var existingAttributes = await GetByProductIdAsync(sanPhamId, cancellationToken);
            var existingDict = existingAttributes.ToDictionary(x => x.TenThuocTinh, x => x);

            if (deleteOthers)
            {
                var attributesToDelete = existingDict.Keys.Except(newAttributes.Keys);
                foreach (var attrName in attributesToDelete)
                {
                    Remove(existingDict[attrName]);
                }
            }

            foreach (var kvp in newAttributes)
            {
                if (existingDict.TryGetValue(kvp.Key, out var existing))
                {
                    if (string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        Remove(existing);
                    }
                    else
                    {
                        existing.GiaTriThuocTinh = kvp.Value;
                        Update(existing);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(kvp.Value))
                {
                    var newAttribute = new ThuocTinhSanPham
                    {
                        MaSanPham = sanPhamId,
                        TenThuocTinh = kvp.Key,
                        GiaTriThuocTinh = kvp.Value
                    };
                    await AddAsync(newAttribute, cancellationToken);
                }
            }

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

    public async Task<IEnumerable<(int ProductId, Dictionary<string, string?> Attributes)>> GetAllProductAttributesAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default)
    {
        var ids = productIds.ToList();
        if (!ids.Any()) return Enumerable.Empty<(int, Dictionary<string, string?>)>();

        var attributes = await _dbSet
            .Where(x => ids.Contains(x.MaSanPham))
            .GroupBy(x => x.MaSanPham)
            .Select(g => new
            {
                ProductId = g.Key,
                Attributes = g.ToDictionary(x => x.TenThuocTinh, x => x.GiaTriThuocTinh)
            })
            .ToListAsync(cancellationToken);

        return attributes.Select(x => (x.ProductId, x.Attributes));
    }
}