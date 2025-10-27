namespace BilliardShop.Application.Common.DTOs;

public class NguoiDungDto : BaseDto
{
    public string TenDangNhap { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Ho { get; set; }
    public string? Ten { get; set; }
    public string? SoDienThoai { get; set; }
    public DateTime? NgaySinh { get; set; }
    public string? GioiTinh { get; set; }
    public int MaVaiTro { get; set; }
    public string? TenVaiTro { get; set; }
    public bool DaXacThucEmail { get; set; }
    public bool TrangThaiHoatDong { get; set; }
    public DateTime? LanDangNhapCuoi { get; set; }
}

public class CreateNguoiDungDto
{
    public string TenDangNhap { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MatKhau { get; set; } = string.Empty;
    public string? Ho { get; set; }
    public string? Ten { get; set; }
    public string? SoDienThoai { get; set; }
    public DateTime? NgaySinh { get; set; }
    public string? GioiTinh { get; set; }
    public int MaVaiTro { get; set; }
}