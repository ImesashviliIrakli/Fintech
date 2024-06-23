using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.Dtos.Order;

public class OrderDto
{
    public int Id { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public required string Currency { get; set; }
    public OrderStatus Status { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}
