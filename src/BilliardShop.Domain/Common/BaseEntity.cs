using System.ComponentModel.DataAnnotations;

namespace BilliardShop.Domain.Common;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}