using Shared;
using Shared.PaymentService;

namespace PaymentService.Services;

public interface IPaymentService
{
    Task<OrderStatus> ProcessPaymentAsync(CreatePaymentDto payment);
}
