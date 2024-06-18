using AutoMapper;
using IdentityService.Models;
using IdentityService.Repositories;
using Shared.IdentityService;

namespace IdentityService.Services;

public class CompanyService : ICompanyService
{
    private readonly IMapper _mapper;
    private readonly ICompanyRepository _repository;

    public CompanyService(IMapper mapper, ICompanyRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<bool> CheckCompanyAsync(string apiKey, string apiSecret)
    {
        return await _repository.CheckCompanyAsync(apiKey, apiSecret);
    }

    public async Task<CompanyDto> RegisterAsync(string name)
    {
        Company company = new Company
        {
            Name = name,
            APIKey = GenerateApiKey(),
            APISecret = GenerateApiSecret()
        };

        await _repository.RegisterAsync(company);

        return _mapper.Map<CompanyDto>(company);
    }

    private string GenerateApiKey()
    {
        return Guid.NewGuid().ToString();
    }

    private string GenerateApiSecret()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
