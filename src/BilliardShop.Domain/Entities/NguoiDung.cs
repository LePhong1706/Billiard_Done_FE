using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class NguoiDung : AuditableEntity, IActivatable
{
    [Required]
    [StringLength(50)]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string MatKhauMaHoa { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string MuoiMatKhau { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Ho { get; set; }

    [StringLength(50)]
    public string? Ten { get; set; }

    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    public DateTime? NgaySinh { get; set; }

    [StringLength(1)]
    public string? GioiTinh { get; set; } // M, F, K

    public int MaVaiTro { get; set; }

    public bool DaXacThucEmail { get; set; } = false;

    public bool TrangThaiHoatDong { get; set; } = true;

    public DateTime? LanDangNhapCuoi { get; set; }

    // Navigation properties
    public virtual VaiTroNguoiDung VaiTro { get; set; } = null!;
    public virtual ICollection<DiaChiNguoiDung> DiaChis { get; set; } = new List<DiaChiNguoiDung>();
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    public virtual ICollection<DanhGiaSanPham> DanhGias { get; set; } = new List<DanhGiaSanPham>();
    public virtual ICollection<DanhSachYeuThich> YeuThichs { get; set; } = new List<DanhSachYeuThich>();
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
    public virtual ICollection<SanPham> SanPhamsTao { get; set; } = new List<SanPham>();
    public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
}