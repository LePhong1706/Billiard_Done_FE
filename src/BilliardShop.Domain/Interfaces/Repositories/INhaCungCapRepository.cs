using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface INhaCungCapRepository : IGenericRepository<NhaCungCap>
{
    Task<NhaCungCap?> GetByNameAsync(string tenNhaCungCap, CancellationToken cancellationToken = default);
    Task<IEnumerable<NhaCungCap>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<NhaCungCap>> GetByCountryAsync(string quocGia, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string tenNhaCungCap, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<(IEnumerable<NhaCungCap> Suppliers, int TotalCount)> SearchAsync(
        string? searchTerm = null,
        string? quocGia = null,
        bool? trangThaiHoatDong = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);
    Task<bool> UpdateStatusAsync(int supplierId, bool trangThaiHoatDong, CancellationToken cancellationToken = default);
    Task<IEnumerable<NhaCungCap>> GetTopSuppliersAsync(int take = 10, CancellationToken cancellationToken = default);
}