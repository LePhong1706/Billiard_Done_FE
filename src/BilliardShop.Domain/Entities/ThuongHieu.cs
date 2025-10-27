using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class ThuongHieu : AuditableEntity, IActivatable
{
    [Required]
    [StringLength(100)]
    public string TenThuongHieu { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string DuongDanThuongHieu { get; set; } = string.Empty;

    [StringLength(500)]
    public string? MoTa { get; set; }

    [StringLength(255)]
    public string? LogoThuongHieu { get; set; }

    [StringLength(255)]
    public string? Website { get; set; }

    [StringLength(50)]
    public string? QuocGia { get; set; }

    public bool TrangThaiHoatDong { get; set; } = true;

    // Navigation properties
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}

