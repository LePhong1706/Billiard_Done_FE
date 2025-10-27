using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class DonHang : BaseEntity
{
    [Required]
    [StringLength(20)]
    public string SoDonHang { get; set; } = string.Empty;

    public int? MaNguoiDung { get; set; }

    // Thông tin khách hàng (cho khách vãng lai)
    [StringLength(100)]
    [EmailAddress]
    public string? EmailKhachHang { get; set; }

    [StringLength(20)]
    public string? SoDienThoaiKhachHang { get; set; }

    [StringLength(100)]
    public string? TenKhachHang { get; set; }

    // Số tiền đơn hàng
    [Column(TypeName = "decimal(18,2)")]
    public decimal TongTienHang { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TienGiamGia { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PhiVanChuyen { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TienThue { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TongThanhToan { get; set; } = 0;

    // Địa chỉ
    [StringLength(500)]
    public string? DiaChiGiaoHang { get; set; }

    [StringLength(500)]
    public string? DiaChiThanhToan { get; set; }

    // Trạng thái & Thanh toán
    public int MaTrangThai { get; set; }
    public int? MaPhuongThucThanhToan { get; set; }
    public int? MaPhuongThucVanChuyen { get; set; }

    [StringLength(20)]
    public string TrangThaiThanhToan { get; set; } = "ChoThanhToan"; // ChoThanhToan, DaThanhToan, ThatBai, HoanTien

    // Ngày tháng
    public DateTime NgayDatHang { get; set; } = DateTime.UtcNow;
    public DateTime? NgayYeuCauGiao { get; set; }
    public DateTime? NgayGiaoHang { get; set; }
    public DateTime? NgayNhanHang { get; set; }

    // Ghi chú
    [StringLength(500)]
    public string? GhiChuKhachHang { get; set; }

    [StringLength(500)]
    public string? GhiChuQuanTri { get; set; }

    // Navigation properties
    public virtual NguoiDung? NguoiDung { get; set; }
    public virtual TrangThaiDonHang TrangThai { get; set; } = null!;
    public virtual PhuongThucThanhToan? PhuongThucThanhToan { get; set; }
    public virtual PhuongThucVanChuyen? PhuongThucVanChuyen { get; set; }
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    public virtual ICollection<SuDungMaGiamGia> SuDungMaGiamGias { get; set; } = new List<SuDungMaGiamGia>();
}