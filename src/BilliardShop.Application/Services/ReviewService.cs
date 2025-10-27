using BilliardShop.Application.Interfaces;
using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces;

namespace BilliardShop.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReviewService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<DanhGiaSanPham>> GetProductReviewsAsync(int productId)
    {
        var reviews = await _unitOfWork.DanhGiaSanPhamRepository
            .FindAsync(
                r => r.MaSanPham == productId && r.DaDuyet,
                r => r.NguoiDung
            );

        return reviews.OrderByDescending(r => r.NgayTao);
    }

    public async Task<DanhGiaSanPham?> AddReviewAsync(DanhGiaSanPham review)
    {
        review.NgayTao = DateTime.UtcNow;
        review.DaDuyet = false; // Requires approval

        await _unitOfWork.DanhGiaSanPhamRepository.AddAsync(review);
        await _unitOfWork.SaveChangesAsync();

        return review;
    }

    public async Task<double> GetAverageRatingAsync(int productId)
    {
        var reviews = await _unitOfWork.DanhGiaSanPhamRepository
            .FindAsync(r => r.MaSanPham == productId && r.DaDuyet);

        if (!reviews.Any())
            return 0;

        return reviews.Average(r => r.DiemDanhGia);
    }
}
