using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class ChiTietDonHangRepository : GenericRepository<ChiTietDonHang>, IChiTietDonHangRepository
{
    public ChiTietDonHangRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ChiTietDonHang>> GetByOrderIdAsync(int donHangId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaDonHang == donHangId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ChiTietDonHang>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DonHang).ThenInclude(d => d.NguoiDung)
            .Include(x => x.DonHang).ThenInclude(d => d.TrangThai)
            .Where(x => x.MaSanPham == sanPhamId)
            .OrderByDescending(x => x.DonHang.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<ChiTietDonHang?> GetOrderItemAsync(int donHangId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .FirstOrDefaultAsync(x => x.MaDonHang == donHangId && x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<int> CountItemsByOrderAsync(int donHangId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(x => x.MaDonHang == donHangId, cancellationToken);
    }

    public async Task<decimal> GetOrderTotalAsync(int donHangId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaDonHang == donHangId)
            .SumAsync(x => x.ThanhTien, cancellationToken);
    }

    public async Task<int> GetTotalQuantityByOrderAsync(int donHangId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaDonHang == donHangId)
            .SumAsync(x => x.SoLuong, cancellationToken);
    }

    public async Task<IEnumerable<ChiTietDonHang>> GetItemsByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DonHang)
            .Include(x => x.SanPham)
            .Where(x => x.DonHang.NgayDatHang >= fromDate && x.DonHang.NgayDatHang <= toDate)
            .OrderByDescending(x => x.DonHang.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<(int ProductId, string ProductName, int TotalQuantity, decimal TotalRevenue)>> GetTopSellingProductsAsync(int top = 10, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.DonHang)
            .Include(x => x.SanPham)
            .Where(x => x.DonHang.TrangThaiThanhToan == "DaThanhToan");

        if (fromDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang <= toDate.Value);

        var results = await query
            .GroupBy(x => new { x.MaSanPham, x.TenSanPham })
            .Select(g => new
            {
                ProductId = g.Key.MaSanPham,
                ProductName = g.Key.TenSanPham,
                TotalQuantity = g.Sum(x => x.SoLuong),
                TotalRevenue = g.Sum(x => x.ThanhTien)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(top)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.ProductId, x.ProductName, x.TotalQuantity, x.TotalRevenue));
    }

    public async Task<IEnumerable<ChiTietDonHang>> GetItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DonHang)
            .Include(x => x.SanPham)
            .Where(x => x.DonGia >= minPrice && x.DonGia <= maxPrice)
            .OrderByDescending(x => x.DonGia)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateQuantityAsync(int chiTietDonHangId, int newQuantity, CancellationToken cancellationToken = default)
    {
        var item = await GetByIdAsync(chiTietDonHangId, cancellationToken);
        if (item == null) return false;

        item.SoLuong = newQuantity;
        item.ThanhTien = newQuantity * item.DonGia;
        Update(item);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdatePriceAsync(int chiTietDonHangId, decimal newPrice, CancellationToken cancellationToken = default)
    {
        var item = await GetByIdAsync(chiTietDonHangId, cancellationToken);
        if (item == null) return false;

        item.DonGia = newPrice;
        item.ThanhTien = item.SoLuong * newPrice;
        Update(item);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> DeleteByOrderIdAsync(int donHangId, CancellationToken cancellationToken = default)
    {
        var items = await _dbSet
            .Where(x => x.MaDonHang == donHangId)
            .ToListAsync(cancellationToken);

        if (!items.Any()) return 0;

        RemoveRange(items);
        await _context.SaveChangesAsync(cancellationToken);
        return items.Count;
    }

    public async Task<decimal> GetProductRevenueAsync(int sanPhamId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.DonHang)
            .Where(x => x.MaSanPham == sanPhamId && x.DonHang.TrangThaiThanhToan == "DaThanhToan");

        if (fromDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang <= toDate.Value);

        return await query.SumAsync(x => x.ThanhTien, cancellationToken);
    }

    public async Task<int> GetProductSoldQuantityAsync(int sanPhamId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.DonHang)
            .Where(x => x.MaSanPham == sanPhamId && x.DonHang.TrangThaiThanhToan == "DaThanhToan");

        if (fromDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang <= toDate.Value);

        return await query.SumAsync(x => x.SoLuong, cancellationToken);
    }

    public async Task<IEnumerable<ChiTietDonHang>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DonHang)
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Where(x => x.DonHang.MaNguoiDung == nguoiDungId)
            .OrderByDescending(x => x.DonHang.NgayDatHang)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasUserPurchasedProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DonHang)
            .AnyAsync(x => x.DonHang.MaNguoiDung == nguoiDungId &&
                          x.MaSanPham == sanPhamId &&
                          x.DonHang.TrangThaiThanhToan == "DaThanhToan", cancellationToken);
    }

    public async Task<IEnumerable<(int CategoryId, string CategoryName, decimal Revenue)>> GetRevenueByCategoryAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.DonHang)
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Where(x => x.DonHang.TrangThaiThanhToan == "DaThanhToan");

        if (fromDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang <= toDate.Value);

        var results = await query
            .GroupBy(x => new { x.SanPham.DanhMuc.Id, x.SanPham.DanhMuc.TenDanhMuc })
            .Select(g => new
            {
                CategoryId = g.Key.Id,
                CategoryName = g.Key.TenDanhMuc,
                Revenue = g.Sum(x => x.ThanhTien)
            })
            .OrderByDescending(x => x.Revenue)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.CategoryId, x.CategoryName, x.Revenue));
    }

    public async Task<IEnumerable<(int BrandId, string BrandName, decimal Revenue)>> GetRevenueByBrandAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.DonHang)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.DonHang.TrangThaiThanhToan == "DaThanhToan" && x.SanPham.ThuongHieu != null);

        if (fromDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.DonHang.NgayDatHang <= toDate.Value);

        var results = await query
            .GroupBy(x => new { x.SanPham.ThuongHieu!.Id, x.SanPham.ThuongHieu.TenThuongHieu })
            .Select(g => new
            {
                BrandId = g.Key.Id,
                BrandName = g.Key.TenThuongHieu,
                Revenue = g.Sum(x => x.ThanhTien)
            })
            .OrderByDescending(x => x.Revenue)
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.BrandId, x.BrandName, x.Revenue));
    }

    public async Task<decimal> GetAverageOrderItemValueAsync(CancellationToken cancellationToken = default)
    {
        var items = await _dbSet.Select(x => x.ThanhTien).ToListAsync(cancellationToken);
        return items.Any() ? items.Average() : 0;
    }

    public async Task<IEnumerable<ChiTietDonHang>> GetRecentPurchasesAsync(int nguoiDungId, int take = 5, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.DonHang)
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.DonHang.MaNguoiDung == nguoiDungId)
            .OrderByDescending(x => x.DonHang.NgayDatHang)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}