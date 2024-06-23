using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Payment;

public class PaymentDto
{
    [Required]
    public int OrderId { get; set; }
    [Required]
    public required string CardNumber { get; set; }
    [Required]
    public DateTime ExpiryDate { get; set; }
}
