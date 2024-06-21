using OrderService.Models;
using Shared.OrderService;

namespace OrderService.Repositories;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<List<Order>> GetAllCompanyOrdersAsync(int companyId);
    Task<Order> CompleteOrderAsync(int orderId);
}
