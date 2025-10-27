using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class GioHangRepository : GenericRepository<GioHang>, IGioHangRepository
{
    public GioHangRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<GioHang>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaNguoiDung == nguoiDungId && x.SanPham.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayCapNhatCuoi)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<GioHang>> GetBySessionIdAsync(string maPhienLamViec, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham).ThenInclude(p => p.HinhAnhs.Where(h => h.LaHinhAnhChinh))
            .Include(x => x.SanPham).ThenInclude(p => p.DanhMuc)
            .Include(x => x.SanPham).ThenInclude(p => p.ThuongHieu)
            .Where(x => x.MaPhienLamViec == maPhienLamViec && x.SanPham.TrangThaiHoatDong)
            .OrderByDescending(x => x.NgayCapNhatCuoi)
            .ToListAsync(cancellationToken);
    }

    public async Task<GioHang?> GetCartItemAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .FirstOrDefaultAsync(x => x.MaNguoiDung == nguoiDungId && x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<GioHang?> GetCartItemBySessionAsync(string maPhienLamViec, int sanPhamId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.SanPham)
            .FirstOrDefaultAsync(x => x.MaPhienLamViec == maPhienLamViec && x.MaSanPham == sanPhamId, cancellationToken);
    }

    public async Task<int> CountItemsByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .CountAsync(x => x.MaNguoiDung == nguoiDungId, cancellationToken);
    }

    public async Task<int> CountItemsBySessionAsync(string maPhienLamViec, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .CountAsync(x => x.MaPhienLamViec == maPhienLamViec, cancellationToken);
    }

    public async Task<decimal> GetTotalValueByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        var items = await _dbSet
            .Include(x => x.SanPham)
            .Where(x => x.MaNguoiDung == nguoiDungId && x.SanPham.TrangThaiHoatDong)
            .ToListAsync(cancellationToken);

        return items.Sum(x => x.SoLuong * (x.SanPham.GiaKhuyenMai ?? x.SanPham.GiaGoc));
    }

    public async Task<decimal> GetTotalValueBySessionAsync(string maPhienLamViec, CancellationToken cancellationToken = default)
    {
        var items = await _dbSet
            .Include(x => x.SanPham)
            .Where(x => x.MaPhienLamViec == maPhienLamViec && x.SanPham.TrangThaiHoatDong)
            .ToListAsync(cancellationToken);

        return items.Sum(x => x.SoLuong * (x.SanPham.GiaKhuyenMai ?? x.SanPham.GiaGoc));
    }

    public async Task<bool> AddToCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId, int soLuong, CancellationToken cancellationToken = default)
    {
        var product = await _context.SanPhams
            .FirstOrDefaultAsync(x => x.Id == sanPhamId && x.TrangThaiHoatDong, cancellationToken);

        if (product == null || product.SoLuongTonKho < soLuong)
            return false;

        GioHang? existingItem = null;

        if (nguoiDungId.HasValue)
        {
            existingItem = await GetCartItemAsync(nguoiDungId.Value, sanPhamId, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            existingItem = await GetCartItemBySessionAsync(maPhienLamViec, sanPhamId, cancellationToken);
        }

        if (existingItem != null)
        {
            var newQuantity = existingItem.SoLuong + soLuong;
            if (product.SoLuongTonKho < newQuantity)
                return false;

            existingItem.SoLuong = newQuantity;
            existingItem.NgayCapNhatCuoi = DateTime.UtcNow;
            Update(existingItem);
        }
        else
        {
            var newCartItem = new GioHang
            {
                MaNguoiDung = nguoiDungId,
                MaPhienLamViec = maPhienLamViec,
                MaSanPham = sanPhamId,
                SoLuong = soLuong,
                NgayTao = DateTime.UtcNow,
                NgayCapNhatCuoi = DateTime.UtcNow
            };

            await AddAsync(newCartItem, cancellationToken);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateQuantityAsync(int gioHangId, int newQuantity, CancellationToken cancellationToken = default)
    {
        if (newQuantity <= 0) return false;

        var cartItem = await _dbSet
            .Include(x => x.SanPham)
            .FirstOrDefaultAsync(x => x.Id == gioHangId, cancellationToken);

        if (cartItem == null || !cartItem.SanPham.TrangThaiHoatDong || cartItem.SanPham.SoLuongTonKho < newQuantity)
            return false;

        cartItem.SoLuong = newQuantity;
        cartItem.NgayCapNhatCuoi = DateTime.UtcNow;
        Update(cartItem);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveFromCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId, CancellationToken cancellationToken = default)
    {
        GioHang? cartItem = null;

        if (nguoiDungId.HasValue)
        {
            cartItem = await GetCartItemAsync(nguoiDungId.Value, sanPhamId, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            cartItem = await GetCartItemBySessionAsync(maPhienLamViec, sanPhamId, cancellationToken);
        }

        if (cartItem == null) return false;

        Remove(cartItem);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> ClearCartByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        var cartItems = await _dbSet
            .Where(x => x.MaNguoiDung == nguoiDungId)
            .ToListAsync(cancellationToken);

        if (!cartItems.Any()) return 0;

        RemoveRange(cartItems);
        await _context.SaveChangesAsync(cancellationToken);
        return cartItems.Count;
    }

    public async Task<int> ClearCartBySessionAsync(string maPhienLamViec, CancellationToken cancellationToken = default)
    {
        var cartItems = await _dbSet
            .Where(x => x.MaPhienLamViec == maPhienLamViec)
            .ToListAsync(cancellationToken);

        if (!cartItems.Any()) return 0;

        RemoveRange(cartItems);
        await _context.SaveChangesAsync(cancellationToken);
        return cartItems.Count;
    }

    public async Task<bool> MergeCartAsync(string maPhienLamViec, int nguoiDungId, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var sessionCartItems = await GetBySessionIdAsync(maPhienLamViec, cancellationToken);
            var userCartItems = await GetByUserIdAsync(nguoiDungId, cancellationToken);

            var userCartDict = userCartItems.ToDictionary(x => x.MaSanPham, x => x);

            foreach (var sessionItem in sessionCartItems)
            {
                if (userCartDict.TryGetValue(sessionItem.MaSanPham, out var userItem))
                {
                    var totalQuantity = userItem.SoLuong + sessionItem.SoLuong;
                    if (sessionItem.SanPham.SoLuongTonKho >= totalQuantity)
                    {
                        userItem.SoLuong = totalQuantity;
                        userItem.NgayCapNhatCuoi = DateTime.UtcNow;
                        Update(userItem);
                    }
                    Remove(sessionItem);
                }
                else
                {
                    sessionItem.MaNguoiDung = nguoiDungId;
                    sessionItem.MaPhienLamViec = null;
                    sessionItem.NgayCapNhatCuoi = DateTime.UtcNow;
                    Update(sessionItem);
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

    public async Task<bool> TransferCartToUserAsync(string maPhienLamViec, int nguoiDungId, CancellationToken cancellationToken = default)
    {
        var cartItems = await _dbSet
            .Where(x => x.MaPhienLamViec == maPhienLamViec)
            .ToListAsync(cancellationToken);

        if (!cartItems.Any()) return true;

        foreach (var item in cartItems)
        {
            item.MaNguoiDung = nguoiDungId;
            item.MaPhienLamViec = null;
            item.NgayCapNhatCuoi = DateTime.UtcNow;
            Update(item);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<GioHang>> GetExpiredCartsAsync(int daysOld = 30, CancellationToken cancellationToken = default)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
        return await _dbSet
            .Where(x => x.MaNguoiDung == null && x.NgayCapNhatCuoi < cutoffDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CleanupExpiredCartsAsync(int daysOld = 30, CancellationToken cancellationToken = default)
    {
        var expiredCarts = await GetExpiredCartsAsync(daysOld, cancellationToken);
        if (!expiredCarts.Any()) return 0;

        RemoveRange(expiredCarts);
        await _context.SaveChangesAsync(cancellationToken);
        return expiredCarts.Count();
    }

    public async Task<bool> ValidateCartItemsAsync(int? nguoiDungId, string? maPhienLamViec, CancellationToken cancellationToken = default)
    {
        IEnumerable<GioHang> cartItems;

        if (nguoiDungId.HasValue)
        {
            cartItems = await GetByUserIdAsync(nguoiDungId.Value, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            cartItems = await GetBySessionIdAsync(maPhienLamViec, cancellationToken);
        }
        else
        {
            return true;
        }

        var hasChanges = false;
        foreach (var item in cartItems)
        {
            if (!item.SanPham.TrangThaiHoatDong || item.SanPham.SoLuongTonKho == 0)
            {
                Remove(item);
                hasChanges = true;
            }
            else if (item.SoLuong > item.SanPham.SoLuongTonKho)
            {
                item.SoLuong = item.SanPham.SoLuongTonKho;
                item.NgayCapNhatCuoi = DateTime.UtcNow;
                Update(item);
                hasChanges = true;
            }
        }

        if (hasChanges)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    public async Task<IEnumerable<GioHang>> GetInvalidCartItemsAsync(int? nguoiDungId, string? maPhienLamViec, CancellationToken cancellationToken = default)
    {
        IQueryable<GioHang> query = _dbSet.Include(x => x.SanPham);

        if (nguoiDungId.HasValue)
        {
            query = query.Where(x => x.MaNguoiDung == nguoiDungId.Value);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            query = query.Where(x => x.MaPhienLamViec == maPhienLamViec);
        }
        else
        {
            return Enumerable.Empty<GioHang>();
        }

        return await query
            .Where(x => !x.SanPham.TrangThaiHoatDong || 
                       x.SanPham.SoLuongTonKho == 0 || 
                       x.SoLuong > x.SanPham.SoLuongTonKho)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsInCartAsync(int? nguoiDungId, string? maPhienLamViec, int sanPhamId, CancellationToken cancellationToken = default)
    {
        if (nguoiDungId.HasValue)
        {
            return await _dbSet.AnyAsync(x => x.MaNguoiDung == nguoiDungId.Value && x.MaSanPham == sanPhamId, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(maPhienLamViec))
        {
            return await _dbSet.AnyAsync(x => x.MaPhienLamViec == maPhienLamViec && x.MaSanPham == sanPhamId, cancellationToken);
        }
        
        return false;
    }
}