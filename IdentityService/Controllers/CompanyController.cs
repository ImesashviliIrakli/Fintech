using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(string name)
    {
        if (string.IsNullOrEmpty(name))
            return BadRequest("Please provide valid name");

        return Ok(await _companyService.RegisterAsync(name));
    }

    [HttpPost]
    public async Task<IActionResult> CheckCompany(string apiKey, string apiSecret)
    {
        return Ok(await _companyService.CheckCompanyAsync(apiKey, apiSecret));
    }
}
