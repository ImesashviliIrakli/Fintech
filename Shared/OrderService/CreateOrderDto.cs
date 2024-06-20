using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.OrderService;

public class CreateOrderDto
{
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public required string Currency { get; set; }
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
