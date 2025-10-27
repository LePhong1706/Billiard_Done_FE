using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class PhuongThucVanChuyenRepository : GenericRepository<PhuongThucVanChuyen>, IPhuongThucVanChuyenRepository
{
    public PhuongThucVanChuyenRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<PhuongThucVanChuyen?> GetByNameAsync(string tenPhuongThuc, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.TenPhuongThuc == tenPhuongThuc, cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucVanChuyen>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .OrderBy(x => x.PhiCoBan)
            .ThenBy(x => x.TenPhuongThuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string tenPhuongThuc, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TenPhuongThuc == tenPhuongThuc);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> UpdateActiveStatusAsync(int phuongThucId, bool trangThaiHoatDong, CancellationToken cancellationToken = default)
    {
        var phuongThuc = await GetByIdAsync(phuongThucId, cancellationToken);
        if (phuongThuc == null) return false;

        phuongThuc.TrangThaiHoatDong = trangThaiHoatDong;
        Update(phuongThuc);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> CountOrdersByShippingMethodAsync(int phuongThucId, CancellationToken cancellationToken = default)
    {
        return await _context.DonHangs
            .CountAsync(x => x.MaPhuongThucVanChuyen == phuongThucId, cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucVanChuyen>> GetWithOrderCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .GroupJoin(
                _context.DonHangs,
                method => method.Id,
                order => order.MaPhuongThucVanChuyen,
                (method, orders) => new { Method = method, Orders = orders }
            )
            .Select(x => new PhuongThucVanChuyen
            {
                Id = x.Method.Id,
                TenPhuongThuc = x.Method.TenPhuongThuc,
                MoTa = x.Method.MoTa,
                PhiCoBan = x.Method.PhiCoBan,
                PhiTheoTrongLuong = x.Method.PhiTheoTrongLuong,
                SoNgayDuKien = x.Method.SoNgayDuKien,
                TrangThaiHoatDong = x.Method.TrangThaiHoatDong,
                DonHangs = x.Orders.ToList()
            })
            .OrderBy(x => x.PhiCoBan)
            .ThenBy(x => x.TenPhuongThuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasOrdersAsync(int phuongThucId, CancellationToken cancellationToken = default)
    {
        return await _context.DonHangs
            .AnyAsync(x => x.MaPhuongThucVanChuyen == phuongThucId, cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucVanChuyen>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var searchLower = searchTerm.ToLower();
        return await _dbSet
            .Where(x => x.TenPhuongThuc.ToLower().Contains(searchLower) ||
                       (x.MoTa != null && x.MoTa.ToLower().Contains(searchLower)))
            .OrderBy(x => x.PhiCoBan)
            .ThenBy(x => x.TenPhuongThuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucVanChuyen>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong && x.PhiCoBan >= minPrice && x.PhiCoBan <= maxPrice)
            .OrderBy(x => x.PhiCoBan)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucVanChuyen>> GetByDeliveryTimeAsync(int maxDays, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong && 
                       (x.SoNgayDuKien == null || x.SoNgayDuKien <= maxDays))
            .OrderBy(x => x.SoNgayDuKien)
            .ThenBy(x => x.PhiCoBan)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucVanChuyen>> GetFreeShippingMethodsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong && x.PhiCoBan == 0)
            .OrderBy(x => x.TenPhuongThuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucVanChuyen>> GetPopularShippingMethodsAsync(int top = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .GroupJoin(
                _context.DonHangs,
                method => method.Id,
                order => order.MaPhuongThucVanChuyen,
                (method, orders) => new { Method = method, OrderCount = orders.Count() }
            )
            .OrderByDescending(x => x.OrderCount)
            .Take(top)
            .Select(x => x.Method)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> CalculateShippingCostAsync(int phuongThucId, decimal weight = 0, CancellationToken cancellationToken = default)
    {
        var method = await GetByIdAsync(phuongThucId, cancellationToken);
        if (method == null || !method.TrangThaiHoatDong) return 0;

        var cost = method.PhiCoBan;
        if (weight > 0 && method.PhiTheoTrongLuong.HasValue)
        {
            cost += weight * method.PhiTheoTrongLuong.Value;
        }

        return cost;
    }

    public async Task<(IEnumerable<PhuongThucVanChuyen> Methods, int TotalCount)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        bool? trangThaiHoatDong = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (trangThaiHoatDong.HasValue)
        {
            query = query.Where(x => x.TrangThaiHoatDong == trangThaiHoatDong.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(x => x.TenPhuongThuc.ToLower().Contains(searchLower) ||
                                   (x.MoTa != null && x.MoTa.ToLower().Contains(searchLower)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var methods = await query
            .OrderBy(x => x.PhiCoBan)
            .ThenBy(x => x.TenPhuongThuc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (methods, totalCount);
    }
}