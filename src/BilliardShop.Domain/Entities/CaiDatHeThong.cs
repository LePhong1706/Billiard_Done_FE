using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class CaiDatHeThong : AuditableEntity
{
    [Required]
    [StringLength(100)]
    public string KhoaCaiDat { get; set; } = string.Empty;

    public string? GiaTriCaiDat { get; set; }

    [StringLength(255)]
    public string? MoTa { get; set; }

    [StringLength(20)]
    public string KieuDuLieu { get; set; } = "String";

    public int? NguoiCapNhatCuoi { get; set; }

    // Navigation properties
    public virtual NguoiDung? NguoiCapNhat { get; set; }
}