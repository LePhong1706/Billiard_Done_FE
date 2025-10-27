using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class ThuocTinhSanPham : BaseEntity
{
    public int MaSanPham { get; set; }

    [Required]
    [StringLength(100)]
    public string TenThuocTinh { get; set; } = string.Empty;

    [StringLength(255)]
    public string? GiaTriThuocTinh { get; set; }

    // Navigation properties
    public virtual SanPham SanPham { get; set; } = null!;
}