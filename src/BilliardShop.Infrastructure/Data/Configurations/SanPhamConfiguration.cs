using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class SanPhamConfiguration : IEntityTypeConfiguration<SanPham>
{
    public void Configure(EntityTypeBuilder<SanPham> builder)
    {
        // Table name and trigger configuration
        builder.ToTable("SanPham", tb => tb.HasTrigger("tr_SanPham_CapNhatNgayChinhSua"));

        // Relationships
        builder.HasOne(e => e.DanhMuc)
            .WithMany(e => e.SanPhams)
            .HasForeignKey(e => e.MaDanhMuc)
            .HasConstraintName("FK_SanPham_DanhMucSanPham")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ThuongHieu)
            .WithMany(e => e.SanPhams)
            .HasForeignKey(e => e.MaThuongHieu)
            .HasConstraintName("FK_SanPham_ThuongHieu")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.NguoiTao)
            .WithMany(e => e.SanPhamsTao)
            .HasForeignKey(e => e.NguoiTaoMaSanPham)
            .HasConstraintName("FK_SanPham_NguoiTao")
            .OnDelete(DeleteBehavior.SetNull);

        // Unique constraints
        builder.HasIndex(e => e.MaCodeSanPham).IsUnique();
        builder.HasIndex(e => e.DuongDanSanPham).IsUnique();

        // Check constraints
        builder.HasCheckConstraint("CK_SanPham_GiaGoc", "[GiaGoc] >= 0");
        builder.HasCheckConstraint("CK_SanPham_GiaKhuyenMai", "[GiaKhuyenMai] >= 0");
        builder.HasCheckConstraint("CK_SanPham_GiaVon", "[GiaVon] >= 0");
        builder.HasCheckConstraint("CK_SanPham_SoLuongTonKho", "[SoLuongTonKho] >= 0");
    }
}