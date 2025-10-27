using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class CaiDatHeThongConfiguration : IEntityTypeConfiguration<CaiDatHeThong>
{
    public void Configure(EntityTypeBuilder<CaiDatHeThong> builder)
    {
        builder.HasIndex(e => e.KhoaCaiDat).IsUnique();

        builder.HasOne(e => e.NguoiCapNhat)
            .WithMany()
            .HasForeignKey(e => e.NguoiCapNhatCuoi)
            .HasConstraintName("FK_CaiDatHeThong_NguoiCapNhat")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
