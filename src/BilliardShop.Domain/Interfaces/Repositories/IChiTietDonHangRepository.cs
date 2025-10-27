using BilliardShop.Domain.Entities;

namespace BilliardShop.Domain.Interfaces.Repositories;

public interface IChiTietDonHangRepository : IGenericRepository<ChiTietDonHang>
{
    Task<IEnumerable<ChiTietDonHang>> GetByOrderIdAsync(int donHangId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChiTietDonHang>> GetByProductIdAsync(int sanPhamId, CancellationToken cancellationToken = default);
    Task<ChiTietDonHang?> GetOrderItemAsync(int donHangId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<int> CountItemsByOrderAsync(int donHangId, CancellationToken cancellationToken = default);
    Task<decimal> GetOrderTotalAsync(int donHangId, CancellationToken cancellationToken = default);
    Task<int> GetTotalQuantityByOrderAsync(int donHangId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChiTietDonHang>> GetItemsByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int ProductId, string ProductName, int TotalQuantity, decimal TotalRevenue)>> GetTopSellingProductsAsync(int top = 10, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChiTietDonHang>> GetItemsByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
    Task<bool> UpdateQuantityAsync(int chiTietDonHangId, int newQuantity, CancellationToken cancellationToken = default);
    Task<bool> UpdatePriceAsync(int chiTietDonHangId, decimal newPrice, CancellationToken cancellationToken = default);
    Task<int> DeleteByOrderIdAsync(int donHangId, CancellationToken cancellationToken = default);
    Task<decimal> GetProductRevenueAsync(int sanPhamId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<int> GetProductSoldQuantityAsync(int sanPhamId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChiTietDonHang>> GetByUserIdAsync(int nguoiDungId, CancellationToken cancellationToken = default);
    Task<bool> HasUserPurchasedProductAsync(int nguoiDungId, int sanPhamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int CategoryId, string CategoryName, decimal Revenue)>> GetRevenueByCategoryAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<(int BrandId, string BrandName, decimal Revenue)>> GetRevenueByBrandAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<decimal> GetAverageOrderItemValueAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ChiTietDonHang>> GetRecentPurchasesAsync(int nguoiDungId, int take = 5, CancellationToken cancellationToken = default);
}