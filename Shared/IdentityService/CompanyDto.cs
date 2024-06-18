using System.ComponentModel.DataAnnotations;

namespace Shared.IdentityService;

public class CompanyDto
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string APIKey { get; set; }
    [Required]
    public required string APISecret { get; set; }
}