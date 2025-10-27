using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class BaiVietConfiguration : IEntityTypeConfiguration<BaiViet>
{
    public void Configure(EntityTypeBuilder<BaiViet> builder)
    {
        builder.HasOne(e => e.TacGiaNavigation)
            .WithMany(e => e.BaiViets)
            .HasForeignKey(e => e.TacGia)
            .HasConstraintName("FK_BaiViet_TacGia")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.DuongDanBaiViet).IsUnique();
    }
}