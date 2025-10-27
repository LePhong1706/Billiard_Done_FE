using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class MaGiamGia : AuditableEntity, IActivatable
{
    [Required]
    [StringLength(50)]
    public string MaCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TenMaGiamGia { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }

    // Cài đặt giảm giá
    [Required]
    [StringLength(20)]
    public string LoaiGiamGia { get; set; } = string.Empty; // PhanTram, SoTienCoDinh

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaTriGiamGia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaTriDonHangToiThieu { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? SoTienGiamToiDa { get; set; }

    // Giới hạn sử dụng
    public int? SoLuotSuDungToiDa { get; set; }
    public int SoLuotDaSuDung { get; set; } = 0;
    public int SoLuotSuDungToiDaMoiNguoi { get; set; } = 1;

    // Thời gian hiệu lực
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }

    public bool TrangThaiHoatDong { get; set; } = true;

    // Navigation properties
    public virtual ICollection<SuDungMaGiamGia> SuDungMaGiamGias { get; set; } = new List<SuDungMaGiamGia>();
}