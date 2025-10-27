namespace BilliardShop.Application.Common.DTOs;

public class CaiDatHeThongDto : BaseDto
{
    public string KhoaCaiDat { get; set; } = string.Empty;
    public string? GiaTriCaiDat { get; set; }
    public string? MoTa { get; set; }
    public string KieuDuLieu { get; set; } = "String";
    public int? NguoiCapNhatCuoi { get; set; }
}