using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class ChiTietDonHangConfiguration : IEntityTypeConfiguration<ChiTietDonHang>
{
    public void Configure(EntityTypeBuilder<ChiTietDonHang> builder)
    {
        builder.HasOne(e => e.DonHang)
            .WithMany(e => e.ChiTietDonHangs)
            .HasForeignKey(e => e.MaDonHang)
            .HasConstraintName("FK_ChiTietDonHang_DonHang")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.SanPham)
            .WithMany(e => e.ChiTietDonHangs)
            .HasForeignKey(e => e.MaSanPham)
            .HasConstraintName("FK_ChiTietDonHang_SanPham")
            .OnDelete(DeleteBehavior.Restrict);

        // Check constraints
        builder.HasCheckConstraint("CK_ChiTietDonHang_SoLuong", "[SoLuong] > 0");
        builder.HasCheckConstraint("CK_ChiTietDonHang_DonGia", "[DonGia] >= 0");
        builder.HasCheckConstraint("CK_ChiTietDonHang_ThanhTien", "[ThanhTien] >= 0");
    }
}
