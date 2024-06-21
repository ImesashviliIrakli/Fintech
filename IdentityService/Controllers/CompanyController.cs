using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Route("api/companies")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(string name)
    {
        if (string.IsNullOrEmpty(name))
            return BadRequest("Please provide valid name");

        return Ok(await _companyService.RegisterAsync(name));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _companyService.GetCompanyAsync(id));
    }

    [HttpGet("validate")]
    public async Task<IActionResult> ValidateCompanyCredentials()
    {
        if (!Request.Headers.TryGetValue("ApiKey", out var apiKey) ||
               !Request.Headers.TryGetValue("ApiSecret", out var apiSecret))
        {
            return Unauthorized("Missing API key or secret");
        }

        var companyId = await _companyService.ValidateCompanyCredentialsAsync(apiKey, apiSecret);

        return Ok(companyId);
    }
}
