using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class HinhAnhSanPhamConfiguration : IEntityTypeConfiguration<HinhAnhSanPham>
{
    public void Configure(EntityTypeBuilder<HinhAnhSanPham> builder)
    {
        builder.HasOne(e => e.SanPham)
            .WithMany(e => e.HinhAnhs)
            .HasForeignKey(e => e.MaSanPham)
            .HasConstraintName("FK_HinhAnhSanPham_SanPham")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
