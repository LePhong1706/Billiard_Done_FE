using BilliardShop.Domain.Entities;

namespace BilliardShop.Application.Interfaces;

public interface IRecentlyViewedService
{
    Task<IEnumerable<SanPham>> GetRecentlyViewedProductsAsync(List<int> productIds);
    List<int> AddRecentlyViewed(List<int> existingIds, int productId, int maxCount = 10);
}
