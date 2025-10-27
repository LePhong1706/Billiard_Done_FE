using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class BinhLuanBaiVietConfiguration : IEntityTypeConfiguration<BinhLuanBaiViet>
{
    public void Configure(EntityTypeBuilder<BinhLuanBaiViet> builder)
    {
        builder.HasOne(e => e.BaiViet)
            .WithMany(e => e.BinhLuans)
            .HasForeignKey(e => e.MaBaiViet)
            .HasConstraintName("FK_BinhLuanBaiViet_BaiViet")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.NguoiDung)
            .WithMany()
            .HasForeignKey(e => e.MaNguoiDung)
            .HasConstraintName("FK_BinhLuanBaiViet_NguoiDung")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.BinhLuanCha)
            .WithMany(e => e.BinhLuanCons)
            .HasForeignKey(e => e.MaBinhLuanCha)
            .HasConstraintName("FK_BinhLuanBaiViet_BinhLuanCha")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.NguoiDuyetNavigation)
            .WithMany()
            .HasForeignKey(e => e.NguoiDuyet)
            .HasConstraintName("FK_BinhLuanBaiViet_NguoiDuyet")
            .OnDelete(DeleteBehavior.SetNull);
    }
}