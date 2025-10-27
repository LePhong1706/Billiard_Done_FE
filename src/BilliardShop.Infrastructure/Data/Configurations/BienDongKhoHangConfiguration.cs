using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class BienDongKhoHangConfiguration : IEntityTypeConfiguration<BienDongKhoHang>
{
    public void Configure(EntityTypeBuilder<BienDongKhoHang> builder)
    {
        builder.HasOne(e => e.SanPham)
            .WithMany(e => e.BienDongKhoHangs)
            .HasForeignKey(e => e.MaSanPham)
            .HasConstraintName("FK_BienDongKhoHang_SanPham")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.NguoiThucHienNavigation)
            .WithMany()
            .HasForeignKey(e => e.NguoiThucHien)
            .HasConstraintName("FK_BienDongKhoHang_NguoiDung")
            .OnDelete(DeleteBehavior.SetNull);
    }
}