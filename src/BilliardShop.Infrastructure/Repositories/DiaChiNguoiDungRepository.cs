using Microsoft.EntityFrameworkCore;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;

namespace BilliardShop.Infrastructure.Repositories;

public class DiaChiNguoiDungRepository : GenericRepository<DiaChiNguoiDung>, IDiaChiNguoiDungRepository
{
    public DiaChiNguoiDungRepository(BilliardShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DiaChiNguoiDung>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaNguoiDung == nguoiDungId)
            .OrderByDescending(x => x.LaDiaChiMacDinh)
            .ThenByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DiaChiNguoiDung>> GetByUserAndTypeAsync(int nguoiDungId, string loaiDiaChi, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.MaNguoiDung == nguoiDungId && 
                       (x.LoaiDiaChi == loaiDiaChi || x.LoaiDiaChi == "CaHai"))
            .OrderByDescending(x => x.LaDiaChiMacDinh)
            .ThenByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }

    public async Task<DiaChiNguoiDung?> GetDefaultAddressAsync(int nguoiDungId, string? loaiDiaChi = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaNguoiDung == nguoiDungId && x.LaDiaChiMacDinh);

        if (!string.IsNullOrEmpty(loaiDiaChi))
        {
            query = query.Where(x => x.LoaiDiaChi == loaiDiaChi || x.LoaiDiaChi == "CaHai");
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<DiaChiNguoiDung?> GetDefaultShippingAddressAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await GetDefaultAddressAsync(nguoiDungId, "GiaoHang", cancellationToken);
    }

    public async Task<DiaChiNguoiDung?> GetDefaultBillingAddressAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        return await GetDefaultAddressAsync(nguoiDungId, "ThanhToan", cancellationToken);
    }

    public async Task<bool> SetDefaultAddressAsync(int diaChiId, int nguoiDungId, string? loaiDiaChi = null, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Unset all default addresses first
            await UnsetDefaultAddressesAsync(nguoiDungId, loaiDiaChi, diaChiId, cancellationToken);

            // Set the new default address
            var address = await _dbSet
                .FirstOrDefaultAsync(x => x.Id == diaChiId && x.MaNguoiDung == nguoiDungId, cancellationToken);

            if (address == null)
                return false;

            address.LaDiaChiMacDinh = true;
            Update(address);

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

    public async Task<bool> UnsetDefaultAddressesAsync(int nguoiDungId, string? loaiDiaChi = null, int? excludeAddressId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaNguoiDung == nguoiDungId && x.LaDiaChiMacDinh);

        if (!string.IsNullOrEmpty(loaiDiaChi))
        {
            query = query.Where(x => x.LoaiDiaChi == loaiDiaChi || x.LoaiDiaChi == "CaHai");
        }

        if (excludeAddressId.HasValue)
        {
            query = query.Where(x => x.Id != excludeAddressId.Value);
        }

        var addresses = await query.ToListAsync(cancellationToken);

        foreach (var address in addresses)
        {
            address.LaDiaChiMacDinh = false;
            Update(address);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<int> CountByUserAsync(int nguoiDungId, string? loaiDiaChi = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaNguoiDung == nguoiDungId);

        if (!string.IsNullOrEmpty(loaiDiaChi))
        {
            query = query.Where(x => x.LoaiDiaChi == loaiDiaChi || x.LoaiDiaChi == "CaHai");
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> HasAddressAsync(int nguoiDungId, string? loaiDiaChi = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(x => x.MaNguoiDung == nguoiDungId);

        if (!string.IsNullOrEmpty(loaiDiaChi))
        {
            query = query.Where(x => x.LoaiDiaChi == loaiDiaChi || x.LoaiDiaChi == "CaHai");
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<DiaChiNguoiDung>> GetByProvinceAsync(string tinhThanhPho, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.NguoiDung)
            .Where(x => x.TinhThanhPho == tinhThanhPho)
            .OrderBy(x => x.ThanhPho)
            .ThenBy(x => x.QuanHuyen)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> DeleteAllByUserAsync(int nguoiDungId, CancellationToken cancellationToken = default)
    {
        var addresses = await _dbSet
            .Where(x => x.MaNguoiDung == nguoiDungId)
            .ToListAsync(cancellationToken);

        if (!addresses.Any())
            return 0;

        RemoveRange(addresses);
        await _context.SaveChangesAsync(cancellationToken);
        return addresses.Count;
    }

    public async Task<bool> UpdateRecipientInfoAsync(int diaChiId, string? hoTenNguoiNhan, string? soDienThoaiNguoiNhan, CancellationToken cancellationToken = default)
    {
        var address = await GetByIdAsync(diaChiId, cancellationToken);
        if (address == null)
            return false;

        address.HoTenNguoiNhan = hoTenNguoiNhan;
        address.SoDienThoaiNguoiNhan = soDienThoaiNguoiNhan;
        Update(address);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<DiaChiNguoiDung>> SearchAddressesAsync(
        string? searchTerm = null,
        int? nguoiDungId = null,
        string? loaiDiaChi = null,
        string? tinhThanhPho = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Include(x => x.NguoiDung).AsQueryable();

        // Filter by user
        if (nguoiDungId.HasValue)
        {
            query = query.Where(x => x.MaNguoiDung == nguoiDungId.Value);
        }

        // Filter by address type
        if (!string.IsNullOrEmpty(loaiDiaChi))
        {
            query = query.Where(x => x.LoaiDiaChi == loaiDiaChi || x.LoaiDiaChi == "CaHai");
        }

        // Filter by province
        if (!string.IsNullOrEmpty(tinhThanhPho))
        {
            query = query.Where(x => x.TinhThanhPho == tinhThanhPho);
        }

        // Search in address fields
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(x =>
                x.DiaChi.ToLower().Contains(searchLower) ||
                (x.PhuongXa != null && x.PhuongXa.ToLower().Contains(searchLower)) ||
                (x.QuanHuyen != null && x.QuanHuyen.ToLower().Contains(searchLower)) ||
                x.ThanhPho.ToLower().Contains(searchLower) ||
                x.TinhThanhPho.ToLower().Contains(searchLower) ||
                (x.HoTenNguoiNhan != null && x.HoTenNguoiNhan.ToLower().Contains(searchLower)) ||
                (x.SoDienThoaiNguoiNhan != null && x.SoDienThoaiNguoiNhan.Contains(searchTerm))
            );
        }

        return await query
            .OrderByDescending(x => x.LaDiaChiMacDinh)
            .ThenByDescending(x => x.NgayTao)
            .ToListAsync(cancellationToken);
    }
}