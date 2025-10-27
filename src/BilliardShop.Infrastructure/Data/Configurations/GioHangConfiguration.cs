using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class GioHangConfiguration : IEntityTypeConfiguration<GioHang>
{
    public void Configure(EntityTypeBuilder<GioHang> builder)
    {
        builder.HasOne(e => e.NguoiDung)
            .WithMany(e => e.GioHangs)
            .HasForeignKey(e => e.MaNguoiDung)
            .HasConstraintName("FK_GioHang_NguoiDung")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.SanPham)
            .WithMany(e => e.GioHangs)
            .HasForeignKey(e => e.MaSanPham)
            .HasConstraintName("FK_GioHang_SanPham")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasCheckConstraint("CK_GioHang_SoLuong", "[SoLuong] > 0");
    }
}