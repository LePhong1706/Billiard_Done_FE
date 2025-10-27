namespace BilliardShop.Application.Common.DTOs;

public abstract class BaseDto
{
    public int Id { get; set; }
    public DateTime NgayTao { get; set; }
    public DateTime? NgayCapNhatCuoi { get; set; }
}