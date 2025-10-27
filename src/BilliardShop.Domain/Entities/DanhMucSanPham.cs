using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class DanhMucSanPham : AuditableEntity, IActivatable
{
    [Required]
    [StringLength(100)]
    public string TenDanhMuc { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string DuongDanDanhMuc { get; set; } = string.Empty;

    [StringLength(500)]
    public string? MoTa { get; set; }

    public int? MaDanhMucCha { get; set; }

    [StringLength(255)]
    public string? HinhAnhDaiDien { get; set; }

    public int ThuTuSapXep { get; set; } = 0;

    public bool TrangThaiHoatDong { get; set; } = true;

    // Navigation properties
    public virtual DanhMucSanPham? DanhMucCha { get; set; }
    public virtual ICollection<DanhMucSanPham> DanhMucCons { get; set; } = new List<DanhMucSanPham>();
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}