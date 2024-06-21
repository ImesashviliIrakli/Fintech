using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public required string Currency { get; set; }
    public required int Status { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
