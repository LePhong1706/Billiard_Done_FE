using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class DanhMucSanPhamConfiguration : IEntityTypeConfiguration<DanhMucSanPham>
{
    public void Configure(EntityTypeBuilder<DanhMucSanPham> builder)
    {
        builder.HasOne(e => e.DanhMucCha)
            .WithMany(e => e.DanhMucCons)
            .HasForeignKey(e => e.MaDanhMucCha)
            .HasConstraintName("FK_DanhMucSanPham_DanhMucCha")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.DuongDanDanhMuc).IsUnique();
    }
}