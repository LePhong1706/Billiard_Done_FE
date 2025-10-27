using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class PhuongThucVanChuyen : BaseEntity, IActivatable
{
    [Required]
    [StringLength(100)]
    public string TenPhuongThuc { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PhiCoBan { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? PhiTheoTrongLuong { get; set; } = 0;

    public int? SoNgayDuKien { get; set; }

    public bool TrangThaiHoatDong { get; set; } = true;

    // Navigation properties
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
