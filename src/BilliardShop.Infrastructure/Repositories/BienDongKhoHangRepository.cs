using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class BienDongKhoHangRepository : GenericRepository<BienDongKhoHang>, IBienDongKhoHangRepository
{
    public BienDongKhoHangRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BienDongKhoHang>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .Include(x => x.NguoiThucHienNavigation)
            .Where(x => x.MaSanPham == sanPhamId)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BienDongKhoHang>> GetByMovementTypeAsync(string loaiBienDong, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .Include(x => x.NguoiThucHienNavigation)
            .Where(x => x.LoaiBienDong == loaiBienDong)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BienDongKhoHang>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .Include(x => x.NguoiThucHienNavigation)
            .Where(x => x.NgayTao >= fromDate && x.NgayTao <= toDate)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BienDongKhoHang>> GetByReferenceAsync(string thamChieu, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .Include(x => x.NguoiThucHienNavigation)
            .Where(x => x.ThamChieu == thamChieu)
            .OrderByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<BienDongKhoHang> Movements, int TotalCount)> GetInventoryHistoryAsync(
        int? sanPhamId = null,
        string? loaiBienDong = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.SanPham)
            .Include(x => x.NguoiThucHienNavigation)
            .AsQueryable();

        if (sanPhamId.HasValue)
        {
            query = query.Where(x => x.MaSanPham == sanPhamId.Value);
        }

        if (!string.IsNullOrWhiteSpace(loaiBienDong))
        {
            query = query.Where(x => x.LoaiBienDong == loaiBienDong);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.NgayTao >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.NgayTao <= toDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var movements = await query
            .OrderByDescending(x => x.NgayTao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (movements, totalCount);
    }

    public async Task<BienDongKhoHang> CreateMovementAsync(
        int sanPhamId,
        string loaiBienDong,
        int soLuong,
        int tonKhoTruoc,
        int tonKhoSau,
        string? thamChieu = null,
        string? ghiChu = null,
        int? nguoiThucHien = null,
        CancellationToken cancellationToken = default)
    {
        var movement = new BienDongKhoHang
        {
            MaSanPham = sanPhamId,
            LoaiBienDong = loaiBienDong,
            SoLuong = soLuong,
            TonKhoTruoc = tonKhoTruoc,
            TonKhoSau = tonKhoSau,
            ThamChieu = thamChieu,
            GhiChu = ghiChu,
            NguoiThucHien = nguoiThucHien,
            NgayTao = DateTime.UtcNow
        };

        var result = await AddAsync(movement, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return result;
    }

    public async Task<IEnumerable<BienDongKhoHang>> GetRecentMovementsAsync(int days = 7, int take = 50, CancellationToken cancellationToken = default)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);

        return await _dbSet
            .Include(x => x.SanPham)
            .Include(x => x.NguoiThucHienNavigation)
            .Where(x => x.NgayTao >= fromDate)
            .OrderByDescending(x => x.NgayTao)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetMovementStatsByTypeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var stats = await _dbSet
            .Where(x => x.NgayTao >= fromDate && x.NgayTao <= toDate)
            .GroupBy(x => x.LoaiBienDong)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Type, x => x.Count, cancellationToken);

        return stats;
    }

    public async Task<IEnumerable<(int ProductId, string ProductName, int TotalIn, int TotalOut, int NetChange)>> GetProductMovementSummaryAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default)
    {
        var movements = await _dbSet
            .Include(x => x.SanPham)
            .Where(x => x.NgayTao >= fromDate && x.NgayTao <= toDate)
            .GroupBy(x => new { x.MaSanPham, x.SanPham.TenSanPham })
            .Select(g => new
            {
                ProductId = g.Key.MaSanPham,
                ProductName = g.Key.TenSanPham,
                TotalIn = g.Where(x => x.LoaiBienDong == "NHAP").Sum(x => x.SoLuong),
                TotalOut = g.Where(x => x.LoaiBienDong == "XUAT").Sum(x => x.SoLuong),
                Adjustments = g.Where(x => x.LoaiBienDong == "DIEU_CHINH").Sum(x => x.SoLuong)
            })
            .ToListAsync(cancellationToken);

        return movements.Select(m => (
            m.ProductId,
            m.ProductName,
            m.TotalIn,
            m.TotalOut,
            m.TotalIn - m.TotalOut + m.Adjustments
        ));
    }

    public async Task<bool> HasMovementsByProductAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(x => x.MaSanPham == sanPhamId, cancellationToken);
    }
}