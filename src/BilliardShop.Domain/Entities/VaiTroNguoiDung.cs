using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class VaiTroNguoiDung : AuditableEntity, IActivatable
{
    [Required]
    [StringLength(50)]
    public string TenVaiTro { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }

    public bool TrangThaiHoatDong { get; set; } = true;

    // Navigation properties
    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
