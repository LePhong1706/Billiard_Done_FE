using BilliardShop.Domain.Entities;

namespace BilliardShop.Application.Interfaces;

public interface IReviewService
{
    Task<IEnumerable<DanhGiaSanPham>> GetProductReviewsAsync(int productId);
    Task<DanhGiaSanPham?> AddReviewAsync(DanhGiaSanPham review);
    Task<double> GetAverageRatingAsync(int productId);
}
