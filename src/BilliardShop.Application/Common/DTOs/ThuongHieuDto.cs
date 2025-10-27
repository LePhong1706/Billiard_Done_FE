namespace BilliardShop.Application.Common.DTOs;

public class ThuongHieuDto : BaseDto
{
    public string TenThuongHieu { get; set; } = string.Empty;
    public string DuongDanThuongHieu { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public string? LogoThuongHieu { get; set; }
    public string? Website { get; set; }
    public string? QuocGia { get; set; }
    public bool TrangThaiHoatDong { get; set; } = true;
    public int SoLuongSanPham { get; set; }
}