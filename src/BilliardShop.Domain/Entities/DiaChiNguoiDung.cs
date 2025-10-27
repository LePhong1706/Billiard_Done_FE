using System.ComponentModel.DataAnnotations;
using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class DiaChiNguoiDung : AuditableEntity
{
    public int MaNguoiDung { get; set; }

    [Required]
    [StringLength(20)]
    public string LoaiDiaChi { get; set; } = string.Empty; // GiaoHang, ThanhToan, CaHai

    [StringLength(100)]
    public string? HoTenNguoiNhan { get; set; }

    [StringLength(20)]
    public string? SoDienThoaiNguoiNhan { get; set; }

    [Required]
    [StringLength(255)]
    public string DiaChi { get; set; } = string.Empty;

    [StringLength(100)]
    public string? PhuongXa { get; set; }

    [StringLength(100)]
    public string? QuanHuyen { get; set; }

    [Required]
    [StringLength(100)]
    public string ThanhPho { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string TinhThanhPho { get; set; } = string.Empty;

    [StringLength(10)]
    public string? MaBuuDien { get; set; }

    public bool LaDiaChiMacDinh { get; set; } = false;

    // Navigation properties
    public virtual NguoiDung NguoiDung { get; set; } = null!;
}