using System.ComponentModel.DataAnnotations;

namespace Shared.OrderService;

public class OrderDto
{
    public int Id { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public required string Currency { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}
