using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class PhuongThucThanhToanRepository : GenericRepository<PhuongThucThanhToan>, IPhuongThucThanhToanRepository
{
    public PhuongThucThanhToanRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<PhuongThucThanhToan?> GetByNameAsync(string tenPhuongThuc, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.TenPhuongThuc == tenPhuongThuc, cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucThanhToan>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .OrderBy(x => x.TenPhuongThuc)
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

    public async Task<int> CountOrdersByPaymentMethodAsync(int phuongThucId, CancellationToken cancellationToken = default)
    {
        return await _context.DonHangs
            .CountAsync(x => x.MaPhuongThucThanhToan == phuongThucId, cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucThanhToan>> GetWithOrderCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .GroupJoin(
                _context.DonHangs,
                method => method.Id,
                order => order.MaPhuongThucThanhToan,
                (method, orders) => new { Method = method, Orders = orders }
            )
            .Select(x => new PhuongThucThanhToan
            {
                Id = x.Method.Id,
                TenPhuongThuc = x.Method.TenPhuongThuc,
                MoTa = x.Method.MoTa,
                TrangThaiHoatDong = x.Method.TrangThaiHoatDong,
                DonHangs = x.Orders.ToList()
            })
            .OrderBy(x => x.TenPhuongThuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasOrdersAsync(int phuongThucId, CancellationToken cancellationToken = default)
    {
        return await _context.DonHangs
            .AnyAsync(x => x.MaPhuongThucThanhToan == phuongThucId, cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucThanhToan>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var searchLower = searchTerm.ToLower();
        return await _dbSet
            .Where(x => x.TenPhuongThuc.ToLower().Contains(searchLower) ||
                       (x.MoTa != null && x.MoTa.ToLower().Contains(searchLower)))
            .OrderBy(x => x.TenPhuongThuc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PhuongThucThanhToan>> GetPopularPaymentMethodsAsync(int top = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.TrangThaiHoatDong)
            .GroupJoin(
                _context.DonHangs,
                method => method.Id,
                order => order.MaPhuongThucThanhToan,
                (method, orders) => new { Method = method, OrderCount = orders.Count() }
            )
            .OrderByDescending(x => x.OrderCount)
            .Take(top)
            .Select(x => x.Method)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<PhuongThucThanhToan> Methods, int TotalCount)> GetPagedAsync(
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
            .OrderBy(x => x.TenPhuongThuc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (methods, totalCount);
    }
}