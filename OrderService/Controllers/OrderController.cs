using Microsoft.AspNetCore.Mvc;
using OrderService.Services;
using Shared.Dtos.Order;
using Shared.Helpers;

namespace OrderService.Controllers;

[Route("orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        if (HttpContext.Items["CompanyId"] is not int companyId)
            return Unauthorized();

        orderDto.CompanyId = companyId;

        var order = await _orderService.CreateOrderAsync(orderDto);
        return Ok(order);
    }

    [HttpGet("compute")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> Compute()
    {
        if (HttpContext.Items["CompanyId"] is not int companyId)
            return Unauthorized();

        var order = await _orderService.ComputeCompanyOrdersAsync(companyId);
        return Ok(order);
    }

    [HttpGet("{id:int}")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> Get(int id)
    {
        if (HttpContext.Items["CompanyId"] is not int companyId)
            return Unauthorized();

        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }
}
