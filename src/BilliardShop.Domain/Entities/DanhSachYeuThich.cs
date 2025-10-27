using BilliardShop.Domain.Common;

namespace BilliardShop.Domain.Entities;

public class DanhSachYeuThich : AuditableEntity
{
    public int MaNguoiDung { get; set; }
    public int MaSanPham { get; set; }

    // Navigation properties
    public virtual NguoiDung NguoiDung { get; set; } = null!;
    public virtual SanPham SanPham { get; set; } = null!;
}