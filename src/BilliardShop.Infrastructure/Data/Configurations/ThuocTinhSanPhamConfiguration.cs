using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class ThuocTinhSanPhamConfiguration : IEntityTypeConfiguration<ThuocTinhSanPham>
{
    public void Configure(EntityTypeBuilder<ThuocTinhSanPham> builder)
    {
        builder.HasOne(e => e.SanPham)
            .WithMany(e => e.ThuocTinhs)
            .HasForeignKey(e => e.MaSanPham)
            .HasConstraintName("FK_ThuocTinhSanPham_SanPham")
            .OnDelete(DeleteBehavior.Cascade);
    }
}