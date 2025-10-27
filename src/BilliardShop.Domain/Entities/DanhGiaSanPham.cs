using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class DanhGiaSanPham : AuditableEntity
{
    public int MaSanPham { get; set; }
    public int? MaNguoiDung { get; set; }
    public int? MaDonHang { get; set; } // Đảm bảo người dùng đã mua sản phẩm

    public int DiemDanhGia { get; set; } // 1-5

    [StringLength(255)]
    public string? TieuDe { get; set; }

    public string? NoiDungDanhGia { get; set; }

    public bool LaMuaHangXacThuc { get; set; } = false;
    public bool DaDuyet { get; set; } = false;

    public DateTime? NgayDuyet { get; set; }
    public int? NguoiDuyet { get; set; }

    // Navigation properties
    public virtual SanPham SanPham { get; set; } = null!;
    public virtual NguoiDung? NguoiDung { get; set; }
    public virtual DonHang? DonHang { get; set; }
    public virtual NguoiDung? NguoiDuyetNavigation { get; set; }
}