namespace BilliardShop.Web.Models;

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal Total { get; set; }
}

public class CartItemViewModel
{
    public int GioHangId { get; set; }
    public int SanPhamId { get; set; }
    public string TenSanPham { get; set; } = string.Empty;
    public string DuongDanSanPham { get; set; } = string.Empty;
    public string? HinhAnhUrl { get; set; }
    public decimal Gia { get; set; }
    public decimal? GiaKhuyenMai { get; set; }
    public int SoLuong { get; set; }
    public int SoLuongTonKho { get; set; }
    public decimal ThanhTien => (GiaKhuyenMai ?? Gia) * SoLuong;
    public bool HetHang => SoLuongTonKho <= 0;
    public bool VuotQuaTonKho => SoLuong > SoLuongTonKho;
}
