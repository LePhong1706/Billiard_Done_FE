using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class SuDungMaGiamGiaConfiguration : IEntityTypeConfiguration<SuDungMaGiamGia>
{
    public void Configure(EntityTypeBuilder<SuDungMaGiamGia> builder)
    {
        // Ignore properties from AuditableEntity that don't exist in database
        builder.Ignore(e => e.NgayTao);
        builder.Ignore(e => e.NgayCapNhatCuoi);
        builder.Ignore(e => e.NguoiTao);
        builder.Ignore(e => e.NguoiCapNhatCuoi);

        builder.HasOne(e => e.MaGiamGia)
            .WithMany(e => e.SuDungMaGiamGias)
            .HasForeignKey(e => e.MaMaGiamGia)
            .HasConstraintName("FK_SuDungMaGiamGia_MaGiamGia")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.NguoiDung)
            .WithMany()
            .HasForeignKey(e => e.MaNguoiDung)
            .HasConstraintName("FK_SuDungMaGiamGia_NguoiDung")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.DonHang)
            .WithMany(e => e.SuDungMaGiamGias)
            .HasForeignKey(e => e.MaDonHang)
            .HasConstraintName("FK_SuDungMaGiamGia_DonHang")
            .OnDelete(DeleteBehavior.Restrict);
    }
}