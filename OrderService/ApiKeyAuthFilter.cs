using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using OrderService.Services;

namespace OrderService;

public class ApiKeyAuthFilter : IAsyncActionFilter
{
    private readonly IIdentityService _identityService;

    public ApiKeyAuthFilter(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var apiKey) ||
            !context.HttpContext.Request.Headers.TryGetValue("ApiSecret", out var apiSecret))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var companyId = await _identityService.ValidateCompanyCredentialsAsync(apiKey, apiSecret);
        if (companyId == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["CompanyId"] = companyId;
        await next();
    }
}
