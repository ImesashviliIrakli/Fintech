using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models;

public class Company
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string APIKey { get; set; }
    [Required]
    public required string APISecret { get; set; }
}
