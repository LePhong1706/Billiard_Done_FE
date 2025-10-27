using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IPhuongThucThanhToanRepository : IGenericRepository<PhuongThucThanhToan>
{
    Task<PhuongThucThanhToan?> GetByNameAsync(string tenPhuongThuc, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucThanhToan>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string tenPhuongThuc, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateActiveStatusAsync(int phuongThucId, bool trangThaiHoatDong, CancellationToken cancellationToken = default);
    Task<int> CountOrdersByPaymentMethodAsync(int phuongThucId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucThanhToan>> GetWithOrderCountAsync(CancellationToken cancellationToken = default);
    Task<bool> HasOrdersAsync(int phuongThucId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucThanhToan>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucThanhToan>> GetPopularPaymentMethodsAsync(int top = 10, CancellationToken cancellationToken = default);
    Task<(IEnumerable<PhuongThucThanhToan> Methods, int TotalCount)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        bool? trangThaiHoatDong = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
}