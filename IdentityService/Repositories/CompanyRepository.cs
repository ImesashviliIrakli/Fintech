using IdentityService.Data;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;
    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CheckCompanyAsync(string apiKey, string apiSecret)
    {
        return await _context.Companies.AnyAsync(x => x.APIKey.Equals(apiKey) && x.APISecret.Equals(apiSecret));
    }

    public async Task<Company> RegisterAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        return company;
    }
}