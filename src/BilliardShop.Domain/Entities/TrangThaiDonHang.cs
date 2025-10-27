using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class TrangThaiDonHang : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string TenTrangThai { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }

    public int ThuTuSapXep { get; set; } = 0;

    // Navigation properties
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
