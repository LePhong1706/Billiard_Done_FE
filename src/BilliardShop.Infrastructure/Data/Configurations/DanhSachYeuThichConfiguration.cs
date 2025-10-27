using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class DanhSachYeuThichConfiguration : IEntityTypeConfiguration<DanhSachYeuThich>
{
    public void Configure(EntityTypeBuilder<DanhSachYeuThich> builder)
    {
        builder.HasOne(e => e.NguoiDung)
            .WithMany(e => e.YeuThichs)
            .HasForeignKey(e => e.MaNguoiDung)
            .HasConstraintName("FK_DanhSachYeuThich_NguoiDung")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.SanPham)
            .WithMany(e => e.YeuThichs)
            .HasForeignKey(e => e.MaSanPham)
            .HasConstraintName("FK_DanhSachYeuThich_SanPham")
            .OnDelete(DeleteBehavior.Cascade);
    }
}