using System.ComponentModel.DataAnnotations.Schema;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class SuDungMaGiamGia : AuditableEntity
{
    public int MaMaGiamGia { get; set; }
    public int? MaNguoiDung { get; set; }
    public int MaDonHang { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SoTienGiamGia { get; set; }

    public DateTime NgaySuDung { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual MaGiamGia MaGiamGia { get; set; } = null!;
    public virtual NguoiDung? NguoiDung { get; set; }
    public virtual DonHang DonHang { get; set; } = null!;
}