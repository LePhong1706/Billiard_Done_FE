using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class DanhGiaSanPhamConfiguration : IEntityTypeConfiguration<DanhGiaSanPham>
{
    public void Configure(EntityTypeBuilder<DanhGiaSanPham> builder)
    {
        builder.HasOne(e => e.SanPham)
            .WithMany(e => e.DanhGias)
            .HasForeignKey(e => e.MaSanPham)
            .HasConstraintName("FK_DanhGiaSanPham_SanPham")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.NguoiDung)
            .WithMany(e => e.DanhGias)
            .HasForeignKey(e => e.MaNguoiDung)
            .HasConstraintName("FK_DanhGiaSanPham_NguoiDung")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.DonHang)
            .WithMany()
            .HasForeignKey(e => e.MaDonHang)
            .HasConstraintName("FK_DanhGiaSanPham_DonHang")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.NguoiDuyetNavigation)
            .WithMany()
            .HasForeignKey(e => e.NguoiDuyet)
            .HasConstraintName("FK_DanhGiaSanPham_NguoiDuyet")
            .OnDelete(DeleteBehavior.SetNull);
    }
}