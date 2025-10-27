using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface ITrangThaiDonHangRepository : IGenericRepository<TrangThaiDonHang>
{
    Task<TrangThaiDonHang?> GetByNameAsync(string tenTrangThai, CancellationToken cancellationToken = default);
    Task<IEnumerable<TrangThaiDonHang>> GetOrderedAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string tenTrangThai, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateSortOrderAsync(int trangThaiId, int sortOrder, CancellationToken cancellationToken = default);
    Task<int> CountOrdersByStatusAsync(int trangThaiId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TrangThaiDonHang>> GetWithOrderCountAsync(CancellationToken cancellationToken = default);
    Task<TrangThaiDonHang?> GetNextStatusAsync(int currentStatusId, CancellationToken cancellationToken = default);
    Task<TrangThaiDonHang?> GetPreviousStatusAsync(int currentStatusId, CancellationToken cancellationToken = default);
    Task<bool> HasOrdersAsync(int trangThaiId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TrangThaiDonHang>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}