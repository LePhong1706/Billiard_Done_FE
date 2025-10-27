using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BilliardShop.Domain.Interfaces;
using BilliardShop.Domain.Interfaces.Repositories;
using BilliardShop.Infrastructure.Data;
using BilliardShop.Infrastructure.Repositories;
using BilliardShop.Infrastructure.Services;
using BilliardShop.Application.Interfaces.Services;

namespace BilliardShop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database Context
        services.AddDbContext<BilliardShopDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(BilliardShopDbContext).Assembly.FullName));
        });

        // Register Generic Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Register Specific Repositories - Order Management
        services.AddScoped<ITrangThaiDonHangRepository, TrangThaiDonHangRepository>();
        services.AddScoped<IPhuongThucThanhToanRepository, PhuongThucThanhToanRepository>();
        services.AddScoped<IPhuongThucVanChuyenRepository, PhuongThucVanChuyenRepository>();
        services.AddScoped<IDonHangRepository, DonHangRepository>();
        services.AddScoped<IChiTietDonHangRepository, ChiTietDonHangRepository>();

        // Register Specific Repositories - User Management
        services.AddScoped<IVaiTroNguoiDungRepository, VaiTroNguoiDungRepository>();
        services.AddScoped<INguoiDungRepository, NguoiDungRepository>();
        services.AddScoped<IDiaChiNguoiDungRepository, DiaChiNguoiDungRepository>();

        // Register Specific Repositories - Product Management
        services.AddScoped<IDanhMucSanPhamRepository, DanhMucSanPhamRepository>();
        services.AddScoped<IThuongHieuRepository, ThuongHieuRepository>();
        services.AddScoped<ISanPhamRepository, SanPhamRepository>();
        services.AddScoped<IHinhAnhSanPhamRepository, HinhAnhSanPhamRepository>();
        services.AddScoped<IThuocTinhSanPhamRepository, ThuocTinhSanPhamRepository>();

        // Register Specific Repositories - Customer Interactions
        services.AddScoped<IGioHangRepository, GioHangRepository>();
        services.AddScoped<IDanhSachYeuThichRepository, DanhSachYeuThichRepository>();
        services.AddScoped<IDanhGiaSanPhamRepository, DanhGiaSanPhamRepository>();

        // Register Specific Repositories - Promotions & Discounts
        services.AddScoped<IMaGiamGiaRepository, MaGiamGiaRepository>();
        services.AddScoped<ISuDungMaGiamGiaRepository, SuDungMaGiamGiaRepository>();

        // Register Specific Repositories - System
        services.AddScoped<ICaiDatHeThongRepository, CaiDatHeThongRepository>();
        services.AddScoped<INhatKyHeThongRepository, NhatKyHeThongRepository>();
        services.AddScoped<IBaiVietRepository, BaiVietRepository>();
        services.AddScoped<IBinhLuanBaiVietRepository, BinhLuanBaiVietRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Services
        services.AddScoped<ICaiDatHeThongService, CaiDatHeThongService>();
        services.AddScoped<IVaiTroNguoiDungService, VaiTroNguoiDungService>();
        services.AddScoped<INguoiDungService, NguoiDungService>();
        services.AddScoped<IThuongHieuService, ThuongHieuService>();
        services.AddScoped<IDanhMucSanPhamService, DanhMucSanPhamService>();

        return services;
    }
}