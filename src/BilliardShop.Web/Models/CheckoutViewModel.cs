namespace BilliardShop.Web.Models;

public class CheckoutViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal Total { get; set; }

    // Thông tin người nhận
    public string TenNguoiNhan { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string DiaChiGiaoHang { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
}

public class OrderConfirmationViewModel
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string TenNguoiNhan { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string DiaChiGiaoHang { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThaiDonHang { get; set; } = string.Empty;
    public List<OrderItemViewModel> Items { get; set; } = new();
}

public class OrderItemViewModel
{
    public string TenSanPham { get; set; } = string.Empty;
    public int SoLuong { get; set; }
    public decimal Gia { get; set; }
    public decimal ThanhTien { get; set; }
}
