using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class ChiTietDonHang : BaseEntity
{
    public int MaDonHang { get; set; }
    public int MaSanPham { get; set; }

    [Required]
    [StringLength(255)]
    public string TenSanPham { get; set; } = string.Empty; // Lưu tên sản phẩm tại thời điểm đặt hàng

    [StringLength(50)]
    public string? MaCodeSanPham { get; set; }

    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ThanhTien { get; set; }

    // Navigation properties
    public virtual DonHang DonHang { get; set; } = null!;
    public virtual SanPham SanPham { get; set; } = null!;
}