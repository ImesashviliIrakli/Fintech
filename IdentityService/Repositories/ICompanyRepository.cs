using IdentityService.Models;

namespace IdentityService.Repositories;

public interface ICompanyRepository
{
    Task<Company> RegisterAsync(Company company);
    Task<Company> GetCompanyAsync(int companyId);
}
