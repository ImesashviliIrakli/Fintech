﻿using Microsoft.AspNetCore.Mvc;
using OrderService.Services;
using Shared.OrderService;

namespace OrderService.Controllers;

[Route("api/[controller]")]
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

        var order = await _orderService.CreateOrderAsync( orderDto);
        return Ok(order);
    }
}
