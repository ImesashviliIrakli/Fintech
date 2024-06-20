using Shared.OrderService;

namespace OrderService.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto order);
    Task<ComputeOrderDto> ComputeCompanyOrdersAsync(int companyId);
}
