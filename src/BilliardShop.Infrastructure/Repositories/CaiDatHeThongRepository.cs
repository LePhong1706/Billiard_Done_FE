using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class CaiDatHeThongRepository : GenericRepository<CaiDatHeThong>, ICaiDatHeThongRepository
{
    public CaiDatHeThongRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<CaiDatHeThong?> GetByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.KhoaCaiDat == khoaCaiDat, cancellationToken);
    }

    public async Task<string?> GetValueAsync(string khoaCaiDat, string? defaultValue = null, CancellationToken cancellationToken = default)
    {
        var setting = await GetByKeyAsync(khoaCaiDat, cancellationToken);
        return setting?.GiaTriCaiDat ?? defaultValue;
    }

    public async Task<int> GetIntValueAsync(string khoaCaiDat, int defaultValue = 0, CancellationToken cancellationToken = default)
    {
        var value = await GetValueAsync(khoaCaiDat, null, cancellationToken);
        return int.TryParse(value, out var result) ? result : defaultValue;
    }

    public async Task<decimal> GetDecimalValueAsync(string khoaCaiDat, decimal defaultValue = 0, CancellationToken cancellationToken = default)
    {
        var value = await GetValueAsync(khoaCaiDat, null, cancellationToken);
        return decimal.TryParse(value, out var result) ? result : defaultValue;
    }

    public async Task<bool> GetBoolValueAsync(string khoaCaiDat, bool defaultValue = false, CancellationToken cancellationToken = default)
    {
        var value = await GetValueAsync(khoaCaiDat, null, cancellationToken);
        return bool.TryParse(value, out var result) ? result : defaultValue;
    }

    public async Task<bool> SetValueAsync(
        string khoaCaiDat, 
        string? giaTriCaiDat, 
        string? moTa = null, 
        string kieuDuLieu = "String", 
        int? nguoiCapNhat = null, 
        CancellationToken cancellationToken = default)
    {
        var existing = await GetByKeyAsync(khoaCaiDat, cancellationToken);

        if (existing != null)
        {
            existing.GiaTriCaiDat = giaTriCaiDat;
            existing.NguoiCapNhatCuoi = nguoiCapNhat;
            existing.NgayCapNhatCuoi = DateTime.UtcNow;
            
            if (!string.IsNullOrEmpty(moTa))
                existing.MoTa = moTa;
            
            existing.KieuDuLieu = kieuDuLieu;
            
            Update(existing);
        }
        else
        {
            var newSetting = new CaiDatHeThong
            {
                KhoaCaiDat = khoaCaiDat,
                GiaTriCaiDat = giaTriCaiDat,
                MoTa = moTa,
                KieuDuLieu = kieuDuLieu,
                NguoiCapNhatCuoi = nguoiCapNhat,
                NgayTao = DateTime.UtcNow,
                NgayCapNhatCuoi = DateTime.UtcNow
            };

            await AddAsync(newSetting, cancellationToken);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> SetMultipleValuesAsync(
        Dictionary<string, string?> caiDats, 
        int? nguoiCapNhat = null, 
        CancellationToken cancellationToken = default)
    {
        var updateCount = 0;
        
        foreach (var kvp in caiDats)
        {
            if (await SetValueAsync(kvp.Key, kvp.Value, null, "String", nguoiCapNhat, cancellationToken))
            {
                updateCount++;
            }
        }

        return updateCount;
    }

    public async Task<bool> ExistsByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.KhoaCaiDat == khoaCaiDat, cancellationToken);
    }

    public async Task<IEnumerable<CaiDatHeThong>> GetByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.KhoaCaiDat.StartsWith(prefix))
            .OrderBy(x => x.KhoaCaiDat)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CaiDatHeThong>> GetByDataTypeAsync(string kieuDuLieu, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.KieuDuLieu == kieuDuLieu)
            .OrderBy(x => x.KhoaCaiDat)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteByKeyAsync(string khoaCaiDat, CancellationToken cancellationToken = default)
    {
        var setting = await GetByKeyAsync(khoaCaiDat, cancellationToken);
        if (setting == null)
            return false;

        Remove(setting);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Dictionary<string, string?>> GetAllAsDictionaryAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .ToDictionaryAsync(x => x.KhoaCaiDat, x => x.GiaTriCaiDat, cancellationToken);
    }

    public async Task<IEnumerable<CaiDatHeThong>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var searchLower = searchTerm.ToLower();
        return await _dbSet
            .Where(x => x.KhoaCaiDat.ToLower().Contains(searchLower) ||
                       (x.MoTa != null && x.MoTa.ToLower().Contains(searchLower)))
            .OrderBy(x => x.KhoaCaiDat)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ResetToDefaultAsync(
        string khoaCaiDat, 
        string defaultValue, 
        int? nguoiCapNhat = null, 
        CancellationToken cancellationToken = default)
    {
        return await SetValueAsync(khoaCaiDat, defaultValue, null, "String", nguoiCapNhat, cancellationToken);
    }

    public async Task<IEnumerable<CaiDatHeThong>> GetHistoryAsync(
        string khoaCaiDat, 
        DateTime? fromDate = null, 
        DateTime? toDate = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.NguoiCapNhat)
            .Where(x => x.KhoaCaiDat == khoaCaiDat);

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.NgayCapNhatCuoi >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.NgayCapNhatCuoi <= toDate.Value);
        }

        return await query
            .OrderByDescending(x => x.NgayCapNhatCuoi)
            .ToListAsync(cancellationToken);
    }
}