using System.ComponentModel.DataAnnotations;

namespace PaymentService.Models;

public class Payment
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int OrderId { get; set; }
    [Required]
    public required string CardNumber { get; set; }
    [Required]
    public DateTime ExpiryDate { get; set; }
}
