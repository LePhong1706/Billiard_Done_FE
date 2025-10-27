using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class DonHangConfiguration : IEntityTypeConfiguration<DonHang>
{
    public void Configure(EntityTypeBuilder<DonHang> builder)
    {
        // Table name and trigger configuration
        builder.ToTable("DonHang", tb => tb.HasTrigger("tr_DonHang_NhatKyHeThong"));

        // Map Id to MaDonHang column
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("MaDonHang");

        // Relationships
        builder.HasOne(e => e.NguoiDung)
            .WithMany(e => e.DonHangs)
            .HasForeignKey(e => e.MaNguoiDung)
            .HasConstraintName("FK_DonHang_NguoiDung")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.TrangThai)
            .WithMany(e => e.DonHangs)
            .HasForeignKey(e => e.MaTrangThai)
            .HasConstraintName("FK_DonHang_TrangThaiDonHang")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PhuongThucThanhToan)
            .WithMany(e => e.DonHangs)
            .HasForeignKey(e => e.MaPhuongThucThanhToan)
            .HasConstraintName("FK_DonHang_PhuongThucThanhToan")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.PhuongThucVanChuyen)
            .WithMany(e => e.DonHangs)
            .HasForeignKey(e => e.MaPhuongThucVanChuyen)
            .HasConstraintName("FK_DonHang_PhuongThucVanChuyen")
            .OnDelete(DeleteBehavior.SetNull);

        // Unique constraint
        builder.HasIndex(e => e.SoDonHang).IsUnique();
    }
}