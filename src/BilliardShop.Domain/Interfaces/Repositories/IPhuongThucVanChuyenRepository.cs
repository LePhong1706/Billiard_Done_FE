using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IPhuongThucVanChuyenRepository : IGenericRepository<PhuongThucVanChuyen>
{
    Task<PhuongThucVanChuyen?> GetByNameAsync(string tenPhuongThuc, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucVanChuyen>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string tenPhuongThuc, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> UpdateActiveStatusAsync(int phuongThucId, bool trangThaiHoatDong, CancellationToken cancellationToken = default);
    Task<int> CountOrdersByShippingMethodAsync(int phuongThucId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucVanChuyen>> GetWithOrderCountAsync(CancellationToken cancellationToken = default);
    Task<bool> HasOrdersAsync(int phuongThucId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucVanChuyen>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucVanChuyen>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucVanChuyen>> GetByDeliveryTimeAsync(int maxDays, CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucVanChuyen>> GetFreeShippingMethodsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PhuongThucVanChuyen>> GetPopularShippingMethodsAsync(int top = 10, CancellationToken cancellationToken = default);
    Task<decimal> CalculateShippingCostAsync(int phuongThucId, decimal weight = 0, CancellationToken cancellationToken = default);
    Task<(IEnumerable<PhuongThucVanChuyen> Methods, int TotalCount)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        bool? trangThaiHoatDong = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
}