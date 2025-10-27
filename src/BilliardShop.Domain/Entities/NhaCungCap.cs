using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class NhaCungCap : AuditableEntity, IActivatable
{
    [Required]
    [StringLength(255)]
    public string TenNhaCungCap { get; set; } = string.Empty;

    [StringLength(100)]
    public string? NguoiLienHe { get; set; }

    [StringLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(100)]
    public string? ThanhPho { get; set; }

    [StringLength(50)]
    public string? QuocGia { get; set; }

    public bool TrangThaiHoatDong { get; set; } = true;
}
