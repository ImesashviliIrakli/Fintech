using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using Shared;
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

    public async Task<Order> CompleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

        if (order == null)
            throw new NotFoundException($"Order with id:{orderId} not found");

        order.Status = (int)OrderStatus.Completed;

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
    }
}
