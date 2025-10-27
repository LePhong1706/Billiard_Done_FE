using BilliardShop.Domain.Entities;

namespace BilliardShop.Web.Models;

public class HomeViewModel
{
    public IEnumerable<SanPham> FeaturedProducts { get; set; } = new List<SanPham>();
    public IEnumerable<DanhMucSanPham> Categories { get; set; } = new List<DanhMucSanPham>();
}
