using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BilliardShop.Domain.Entities;

namespace BilliardShop.Infrastructure.Data.Configurations;

public class NguoiDungConfiguration : IEntityTypeConfiguration<NguoiDung>
{
    public void Configure(EntityTypeBuilder<NguoiDung> builder)
    {
        // Relationships
        builder.HasOne(e => e.VaiTro)
            .WithMany(e => e.NguoiDungs)
            .HasForeignKey(e => e.MaVaiTro)
            .HasConstraintName("FK_NguoiDung_VaiTroNguoiDung")
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraints
        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.TenDangNhap).IsUnique();
    }
}