using Shared.IdentityService;

namespace IdentityService.Services;

public interface ICompanyService
{
    Task<CompanyDto> RegisterAsync(string name);
    Task<CompanyDto> GetCompanyAsync(int companyId);
}
