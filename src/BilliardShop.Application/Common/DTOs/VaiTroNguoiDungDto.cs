namespace BilliardShop.Application.Common.DTOs;

public class VaiTroNguoiDungDto : BaseDto
{
    public string TenVaiTro { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public bool TrangThaiHoatDong { get; set; } = true;
    public int SoLuongNguoiDung { get; set; }
}