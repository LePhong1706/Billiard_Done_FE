using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class NhatKyHeThongConfiguration : IEntityTypeConfiguration<NhatKyHeThong>
{
    public void Configure(EntityTypeBuilder<NhatKyHeThong> builder)
    {
        builder.HasOne(e => e.NguoiDung)
            .WithMany()
            .HasForeignKey(e => e.MaNguoiDung)
            .HasConstraintName("FK_NhatKyHeThong_NguoiDung")
            .OnDelete(DeleteBehavior.SetNull);
    }
}