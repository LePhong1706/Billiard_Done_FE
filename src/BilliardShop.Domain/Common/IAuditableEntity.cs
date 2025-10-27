namespace BilliardShop.Domain.Common;

public interface IAuditableEntity
{
    DateTime NgayTao { get; set; }
    DateTime? NgayCapNhatCuoi { get; set; }
    int? NguoiTao { get; set; }
    int? NguoiCapNhatCuoi { get; set; }
}
