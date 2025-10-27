using BilliardShop.Domain.Entities;

namespace BilliardShop.Web.Models;

public class ProductDetailViewModel
{
    public SanPham Product { get; set; } = null!;
    public IEnumerable<SanPham> RelatedProducts { get; set; } = new List<SanPham>();
    public bool IsInStock => Product.SoLuongTonKho > 0;
    public decimal CurrentPrice => Product.GiaKhuyenMai ?? Product.GiaGoc;
}
