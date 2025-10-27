using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class MaGiamGiaConfiguration : IEntityTypeConfiguration<MaGiamGia>
{
    public void Configure(EntityTypeBuilder<MaGiamGia> builder)
    {
        builder.HasIndex(e => e.MaCode).IsUnique();

        builder.HasCheckConstraint("CK_MaGiamGia_GiaTriGiamGia", "[GiaTriGiamGia] > 0");
    }
}