using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class NhatKyHeThong : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string TenBang { get; set; } = string.Empty;

    public int MaBanGhi { get; set; }

    [Required]
    [StringLength(20)]
    public string HanhDong { get; set; } = string.Empty; // THEM, SUA, XOA

    public string? GiaTriCu { get; set; }
    public string? GiaTriMoi { get; set; }

    public int? MaNguoiDung { get; set; }

    [StringLength(45)]
    public string? DiaChiIP { get; set; }

    [StringLength(255)]
    public string? ThongTinTrinhDuyet { get; set; }

    public DateTime ThoiGian { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual NguoiDung? NguoiDung { get; set; }
}