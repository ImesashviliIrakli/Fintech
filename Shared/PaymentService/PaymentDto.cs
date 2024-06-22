using System.ComponentModel.DataAnnotations;

namespace Shared.PaymentService;

public class PaymentDto
{
    [Required]
    public int OrderId { get; set; }
    [Required]
    public required string CardNumber { get; set; }
    [Required]
    public DateTime ExpiryDate { get; set; }
}
