using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class SanPham : AuditableEntity, IActivatable
{
    [Required]
    [StringLength(50)]
    public string MaCodeSanPham { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string TenSanPham { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string DuongDanSanPham { get; set; } = string.Empty;

    [StringLength(500)]
    public string? MoTaNgan { get; set; }

    public string? MoTaChiTiet { get; set; }

    public int MaDanhMuc { get; set; }

    public int? MaThuongHieu { get; set; }

    // Giá cả
    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaGoc { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? GiaKhuyenMai { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? GiaVon { get; set; }

    // Tồn kho
    public int SoLuongTonKho { get; set; } = 0;
    public int SoLuongToiThieu { get; set; } = 0;
    public int? SoLuongToiDa { get; set; }

    // Thuộc tính sản phẩm
    [Column(TypeName = "decimal(10,3)")]
    public decimal? TrongLuong { get; set; }

    [StringLength(50)]
    public string? KichThuoc { get; set; }

    [StringLength(100)]
    public string? ChatLieu { get; set; }

    [StringLength(50)]
    public string? MauSac { get; set; }

    [StringLength(50)]
    public string? KichCo { get; set; }

    // SEO
    [StringLength(255)]
    public string? TieuDeSEO { get; set; }

    [StringLength(500)]
    public string? MoTaSEO { get; set; }

    [StringLength(255)]
    public string? TuKhoaSEO { get; set; }

    // Trạng thái
    public bool TrangThaiHoatDong { get; set; } = true;
    public bool LaSanPhamNoiBat { get; set; } = false;
    public bool LaSanPhamKhuyenMai { get; set; } = false;

    public int? NguoiTaoMaSanPham { get; set; }

    // Navigation properties
    public virtual DanhMucSanPham DanhMuc { get; set; } = null!;
    public virtual ThuongHieu? ThuongHieu { get; set; }
    public virtual NguoiDung? NguoiTao { get; set; }
    public virtual ICollection<HinhAnhSanPham> HinhAnhs { get; set; } = new List<HinhAnhSanPham>();
    public virtual ICollection<ThuocTinhSanPham> ThuocTinhs { get; set; } = new List<ThuocTinhSanPham>();
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    public virtual ICollection<DanhGiaSanPham> DanhGias { get; set; } = new List<DanhGiaSanPham>();
    public virtual ICollection<DanhSachYeuThich> YeuThichs { get; set; } = new List<DanhSachYeuThich>();
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
    public virtual ICollection<BienDongKhoHang> BienDongKhoHangs { get; set; } = new List<BienDongKhoHang>();
}
