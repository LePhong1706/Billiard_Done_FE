using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class GioHang : AuditableEntity
{
    public int? MaNguoiDung { get; set; }

    [StringLength(255)]
    public string? MaPhienLamViec { get; set; } // Cho khách vãng lai

    public int MaSanPham { get; set; }
    public int SoLuong { get; set; }

    // Navigation properties
    public virtual NguoiDung? NguoiDung { get; set; }
    public virtual SanPham SanPham { get; set; } = null!;
}