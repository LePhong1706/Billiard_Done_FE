namespace BilliardShop.Application.Common.DTOs;

public class DanhMucSanPhamDto : BaseDto
{
    public string TenDanhMuc { get; set; } = string.Empty;
    public string DuongDanDanhMuc { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public int? MaDanhMucCha { get; set; }
    public string? TenDanhMucCha { get; set; }
    public string? HinhAnhDaiDien { get; set; }
    public int ThuTuSapXep { get; set; }
    public bool TrangThaiHoatDong { get; set; } = true;
    public List<DanhMucSanPhamDto> DanhMucCons { get; set; } = new();
    public int SoLuongSanPham { get; set; }
}
