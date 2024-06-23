using OrderService.Models;

namespace OrderService.Repositories;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<List<Order>> GetAllCompanyOrdersAsync(int companyId);
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<Order> CompleteOrderAsync(int orderId, int status);
}
