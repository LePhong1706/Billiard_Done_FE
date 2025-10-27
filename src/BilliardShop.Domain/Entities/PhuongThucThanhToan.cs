using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class PhuongThucThanhToan : BaseEntity, IActivatable
{
    [Required]
    [StringLength(50)]
    public string TenPhuongThuc { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }

    public bool TrangThaiHoatDong { get; set; } = true;

    // Navigation properties
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}