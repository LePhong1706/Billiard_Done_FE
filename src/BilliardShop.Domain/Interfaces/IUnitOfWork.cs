using BilliardShop.Domain.Entities;
using BilliardShop.Domain.Interfaces.Repositories;

namespace BilliardShop.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // User Management Repositories
    IVaiTroNguoiDungRepository VaiTroNguoiDungRepository { get; }
    INguoiDungRepository NguoiDungRepository { get; }
    IDiaChiNguoiDungRepository DiaChiNguoiDungRepository { get; }

    // Product Management Repositories - Updated with specific interfaces
    IDanhMucSanPhamRepository DanhMucSanPhamRepository { get; }
    IThuongHieuRepository ThuongHieuRepository { get; }
    ISanPhamRepository SanPhamRepository { get; }
    IHinhAnhSanPhamRepository HinhAnhSanPhamRepository { get; }
    IThuocTinhSanPhamRepository ThuocTinhSanPhamRepository { get; }

    // Inventory Management Repositories
    INhaCungCapRepository NhaCungCapRepository { get; }
    IBienDongKhoHangRepository BienDongKhoHangRepository { get; }

    // Order Management Repositories
    ITrangThaiDonHangRepository TrangThaiDonHangRepository { get; }
    IPhuongThucThanhToanRepository PhuongThucThanhToanRepository { get; }
    IPhuongThucVanChuyenRepository PhuongThucVanChuyenRepository { get; }
    IDonHangRepository DonHangRepository { get; } 
    IChiTietDonHangRepository ChiTietDonHangRepository { get; } 

    // Promotions & Discounts Repositories
    IMaGiamGiaRepository MaGiamGiaRepository { get; }
    ISuDungMaGiamGiaRepository SuDungMaGiamGiaRepository { get; }

    // Customer Interactions Repositories
    IDanhGiaSanPhamRepository DanhGiaSanPhamRepository { get; }
    IDanhSachYeuThichRepository DanhSachYeuThichRepository { get; }
    IGioHangRepository GioHangRepository { get; }

    // System Settings & Blog Repositories
    ICaiDatHeThongRepository CaiDatHeThongRepository { get; }
    IBaiVietRepository BaiVietRepository { get; }
    IBinhLuanBaiVietRepository BinhLuanBaiVietRepository { get; }
    INhatKyHeThongRepository NhatKyHeThongRepository { get; }

    // Transaction Methods
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();

    // Transaction Management
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}