using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class TrangThaiDonHangRepository : GenericRepository<TrangThaiDonHang>, ITrangThaiDonHangRepository
{
    public TrangThaiDonHangRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<TrangThaiDonHang?> GetByNameAsync(string tenTrangThai, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.TenTrangThai == tenTrangThai, cancellationToken);
    }

    public async Task<IEnumerable<TrangThaiDonHang>> GetOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.TenTrangThai)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string tenTrangThai, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.TenTrangThai == tenTrangThai);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> UpdateSortOrderAsync(int trangThaiId, int sortOrder, CancellationToken cancellationToken = default)
    {
        var trangThai = await GetByIdAsync(trangThaiId, cancellationToken);
        if (trangThai == null) return false;

        trangThai.ThuTuSapXep = sortOrder;
        Update(trangThai);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> CountOrdersByStatusAsync(int trangThaiId, CancellationToken cancellationToken = default)
    {
        return await _context.DonHangs
            .CountAsync(x => x.MaTrangThai == trangThaiId, cancellationToken);
    }

    public async Task<IEnumerable<TrangThaiDonHang>> GetWithOrderCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .GroupJoin(
                _context.DonHangs,
                status => status.Id,
                order => order.MaTrangThai,
                (status, orders) => new { Status = status, Orders = orders }
            )
            .Select(x => new TrangThaiDonHang
            {
                Id = x.Status.Id,
                TenTrangThai = x.Status.TenTrangThai,
                MoTa = x.Status.MoTa,
                ThuTuSapXep = x.Status.ThuTuSapXep,
                DonHangs = x.Orders.ToList()
            })
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.TenTrangThai)
            .ToListAsync(cancellationToken);
    }

    public async Task<TrangThaiDonHang?> GetNextStatusAsync(int currentStatusId, CancellationToken cancellationToken = default)
    {
        var currentStatus = await GetByIdAsync(currentStatusId, cancellationToken);
        if (currentStatus == null) return null;

        return await _dbSet
            .Where(x => x.ThuTuSapXep > currentStatus.ThuTuSapXep)
            .OrderBy(x => x.ThuTuSapXep)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TrangThaiDonHang?> GetPreviousStatusAsync(int currentStatusId, CancellationToken cancellationToken = default)
    {
        var currentStatus = await GetByIdAsync(currentStatusId, cancellationToken);
        if (currentStatus == null) return null;

        return await _dbSet
            .Where(x => x.ThuTuSapXep < currentStatus.ThuTuSapXep)
            .OrderByDescending(x => x.ThuTuSapXep)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> HasOrdersAsync(int trangThaiId, CancellationToken cancellationToken = default)
    {
        return await _context.DonHangs
            .AnyAsync(x => x.MaTrangThai == trangThaiId, cancellationToken);
    }

    public async Task<IEnumerable<TrangThaiDonHang>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var searchLower = searchTerm.ToLower();
        return await _dbSet
            .Where(x => x.TenTrangThai.ToLower().Contains(searchLower) ||
                       (x.MoTa != null && x.MoTa.ToLower().Contains(searchLower)))
            .OrderBy(x => x.ThuTuSapXep)
            .ThenBy(x => x.TenTrangThai)
            .ToListAsync(cancellationToken);
    }
}