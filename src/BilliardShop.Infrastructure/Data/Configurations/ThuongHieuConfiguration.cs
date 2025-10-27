using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class ThuongHieuConfiguration : IEntityTypeConfiguration<ThuongHieu>
{
    public void Configure(EntityTypeBuilder<ThuongHieu> builder)
    {
        builder.HasIndex(e => e.TenThuongHieu).IsUnique();
        builder.HasIndex(e => e.DuongDanThuongHieu).IsUnique();
    }
}