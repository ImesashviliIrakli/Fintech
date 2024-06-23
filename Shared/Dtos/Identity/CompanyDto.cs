using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Identity;

public class CompanyDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string APIKey { get; set; }
    [Required]
    public required string APISecret { get; set; }
}