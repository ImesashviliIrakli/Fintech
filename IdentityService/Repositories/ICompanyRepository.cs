using IdentityService.Models;

namespace IdentityService.Repositories;

public interface ICompanyRepository
{
    Task<Company> RegisterAsync(Company company);
    Task<bool> CheckCompanyAsync(string apiKey, string apiSecret);
}
