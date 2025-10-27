using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class BinhLuanBaiViet : AuditableEntity
{
    public int MaBaiViet { get; set; }
    public int? MaNguoiDung { get; set; }

    [StringLength(100)]
    public string? TenNguoiBinhLuan { get; set; } // Cho khách vãng lai

    [StringLength(100)]
    [EmailAddress]
    public string? EmailNguoiBinhLuan { get; set; }

    [Required]
    [StringLength(1000)]
    public string NoiDungBinhLuan { get; set; } = string.Empty;

    public int? MaBinhLuanCha { get; set; } // Cho reply

    [StringLength(20)]
    public string TrangThai { get; set; } = "ChoDuyet"; // ChoDuyet, DaDuyet, BiTuChoi

    public DateTime? NgayDuyet { get; set; }
    public int? NguoiDuyet { get; set; }

    // Navigation properties
    public virtual BaiViet BaiViet { get; set; } = null!;
    public virtual NguoiDung? NguoiDung { get; set; }
    public virtual BinhLuanBaiViet? BinhLuanCha { get; set; }
    public virtual ICollection<BinhLuanBaiViet> BinhLuanCons { get; set; } = new List<BinhLuanBaiViet>();
    public virtual NguoiDung? NguoiDuyetNavigation { get; set; }
}