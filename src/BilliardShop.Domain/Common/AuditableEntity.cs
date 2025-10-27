namespace BilliardShop.Domain.Common;

public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    public DateTime NgayTao { get; set; } = DateTime.UtcNow;
    public DateTime? NgayCapNhatCuoi { get; set; }
    public int? NguoiTao { get; set; }
    public int? NguoiCapNhatCuoi { get; set; }
}