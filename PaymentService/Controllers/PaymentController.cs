using Microsoft.AspNetCore.Mvc;
using PaymentService.Services;
using Shared.OrderService;
using Shared.PaymentService;

namespace PaymentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> Process([FromBody] PaymentDto paymentDto)
    {
        if (!HttpContext.Request.Headers.TryGetValue("ApiKey", out var apiKey) ||
            !HttpContext.Request.Headers.TryGetValue("ApiSecret", out var apiSecret))
        {
            return Unauthorized();
        }

        var payment = await _paymentService.ProcessPaymentAsync(paymentDto, apiKey, apiSecret);
        return Ok(payment);
    }
}
