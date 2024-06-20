using AutoMapper;
using IdentityService.Models;
using IdentityService.Repositories;
using Shared;
using Shared.Exceptions;
using Shared.IdentityService;

namespace IdentityService.Services;

public class CompanyService : ICompanyService
{
    private readonly EncryptionHelper _encryptionHelper;
    private readonly IMapper _mapper;
    private readonly ICompanyRepository _repository;

    public CompanyService(IMapper mapper, ICompanyRepository repository)
    {
        string encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY");
        string encryptionIv = Environment.GetEnvironmentVariable("ENCRYPTION_IV");

        _encryptionHelper = new EncryptionHelper(encryptionKey, encryptionIv);

        _mapper = mapper;
        _repository = repository;
    }
    public async Task<CompanyDto> GetCompanyAsync(int companyId)
    {
        var company = await _repository.GetCompanyAsync(companyId);

        if (company == null)
            throw new BadRequestException($"Company with id:{companyId} not found");

        company.APIKey = _encryptionHelper.Decrypt(company.APIKey);
        company.APISecret = _encryptionHelper.Decrypt(company.APISecret);

        return _mapper.Map<CompanyDto>(company);
    }

    public async Task<CompanyDto> RegisterAsync(string name)
    {
        Company company = new Company
        {
            Name = name,
            APIKey = _encryptionHelper.Encrypt(GenerateApiKey()),
            APISecret = _encryptionHelper.Encrypt(GenerateApiSecret())
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
