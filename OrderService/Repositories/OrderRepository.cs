﻿using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using Shared.Enums;
using Shared.Exceptions;

namespace OrderService.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Order>> GetAllCompanyOrdersAsync(int companyId)
    {
        return await _context.Orders.Where(x => x.CompanyId == companyId).ToListAsync();
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Order> CompleteOrderAsync(int orderId, int status)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

        if (order == null)
            throw new NotFoundException($"Order with id:{orderId} not found");

        if (order != null && order.Status != 0)
            throw new BadRequestException($"Order with id:{orderId} has already been processed");

        order.Status = status;

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
    }
}
