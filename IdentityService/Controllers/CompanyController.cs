using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace IdentityService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private ResponseDto _response;
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
        _response = new(Status.Success, "Success");
    }

    [HttpPost]
    public async Task<IActionResult> Register(string name)
    {
        _response.Result = await _companyService.RegisterAsync(name);
        return Ok(_response);
    }

    [HttpPost]
    public async Task<IActionResult> CheckCompany(string apiKey, string apiSecret)
    {
        _response.Result = await _companyService.CheckCompanyAsync(apiKey, apiSecret);
        return Ok(_response);
    }
}
