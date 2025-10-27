using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class BienDongKhoHang : AuditableEntity
{
    public int MaSanPham { get; set; }

    [Required]
    [StringLength(20)]
    public string LoaiBienDong { get; set; } = string.Empty; // NHAP, XUAT, DIEU_CHINH

    public int SoLuong { get; set; }
    public int TonKhoTruoc { get; set; }
    public int TonKhoSau { get; set; }

    [StringLength(100)]
    public string? ThamChieu { get; set; } // Mã đơn hàng, lý do điều chỉnh, v.v.

    [StringLength(255)]
    public string? GhiChu { get; set; }

    public int? NguoiThucHien { get; set; }

    // Navigation properties
    public virtual SanPham SanPham { get; set; } = null!;
    public virtual NguoiDung? NguoiThucHienNavigation { get; set; }
}