using Shared.Dtos.Identity;

namespace IdentityService.Services;

public interface ICompanyService
{
    Task<CompanyDto> RegisterAsync(string name);
    Task<CompanyDto> GetCompanyAsync(int companyId);
    Task<int?> ValidateCompanyCredentialsAsync(string apiKey, string apiSecret);
}
