using Shared.Dtos.Order;
using Shared.Dtos.Payment;

namespace OrderService.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto);
    Task<ComputeOrderDto> ComputeCompanyOrdersAsync(int companyId);
    Task<OrderDto> GetOrderByIdAsync(int orderId);
    Task CompleteOrderAsync(PaymentStatusDto paymenStatusDto);
}
