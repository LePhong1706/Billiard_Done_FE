using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class BaiViet : AuditableEntity
{
    [Required]
    [StringLength(255)]
    public string TieuDe { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string DuongDanBaiViet { get; set; } = string.Empty;

    [StringLength(500)]
    public string? TomTat { get; set; }

    public string? NoiDung { get; set; }

    [StringLength(255)]
    public string? HinhAnhDaiDien { get; set; }

    // SEO
    [StringLength(255)]
    public string? TieuDeSEO { get; set; }

    [StringLength(500)]
    public string? MoTaSEO { get; set; }

    [StringLength(255)]
    public string? TuKhoaSEO { get; set; }

    // Thông tin tác giả
    public int TacGia { get; set; }
    public DateTime? NgayXuatBan { get; set; }

    // Trạng thái
    [StringLength(20)]
    public string TrangThai { get; set; } = "NhapBan"; // NhapBan, ChoXuatBan, XuatBan

    public bool NoiBat { get; set; } = false;
    public int LuotXem { get; set; } = 0;

    // Navigation properties
    public virtual NguoiDung TacGiaNavigation { get; set; } = null!;
    public virtual ICollection<BinhLuanBaiViet> BinhLuans { get; set; } = new List<BinhLuanBaiViet>();
}