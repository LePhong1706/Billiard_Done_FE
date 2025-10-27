using BilliardShop.Application.Interfaces;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Application.Services;

public class RecentlyViewedService : IRecentlyViewedService
{
    private readonly IUnitOfWork _unitOfWork;

    public RecentlyViewedService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SanPham>> GetRecentlyViewedProductsAsync(List<int> productIds)
    {
        if (productIds == null || !productIds.Any())
            return Enumerable.Empty<SanPham>();

        var products = await _unitOfWork.SanPhamRepository
            .FindAsync(
                p => productIds.Contains(p.Id) && p.TrangThaiHoatDong,
                p => p.HinhAnhs,
                p => p.DanhMuc
            );

        // Maintain the order of productIds
        return productIds
            .Select(id => products.FirstOrDefault(p => p.Id == id))
            .Where(p => p != null)
            .Cast<SanPham>();
    }

    public List<int> AddRecentlyViewed(List<int> existingIds, int productId, int maxCount = 10)
    {
        existingIds ??= new List<int>();

        // Remove if already exists
        existingIds.Remove(productId);

        // Add to front
        existingIds.Insert(0, productId);

        // Keep only the most recent items
        if (existingIds.Count > maxCount)
        {
            existingIds = existingIds.Take(maxCount).ToList();
        }

        return existingIds;
    }
}
