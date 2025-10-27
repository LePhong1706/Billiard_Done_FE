using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class HinhAnhSanPham : AuditableEntity
{
    public int MaSanPham { get; set; }

    [Required]
    [StringLength(255)]
    public string DuongDanHinhAnh { get; set; } = string.Empty;

    [StringLength(255)]
    public string? TextThayThe { get; set; }

    public int ThuTuSapXep { get; set; } = 0;

    public bool LaHinhAnhChinh { get; set; } = false;

    // Navigation properties
    public virtual SanPham SanPham { get; set; } = null!;
}