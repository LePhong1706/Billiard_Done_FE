using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class DiaChiNguoiDungConfiguration : IEntityTypeConfiguration<DiaChiNguoiDung>
{
    public void Configure(EntityTypeBuilder<DiaChiNguoiDung> builder)
    {
        builder.HasOne(e => e.NguoiDung)
            .WithMany(e => e.DiaChis)
            .HasForeignKey(e => e.MaNguoiDung)
            .HasConstraintName("FK_DiaChiNguoiDung_NguoiDung")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
