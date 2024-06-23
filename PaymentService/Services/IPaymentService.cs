using Shared.Dtos.Payment;
using Shared.Enums;

namespace PaymentService.Services;

public interface IPaymentService
{
    Task<OrderStatus> ProcessPaymentAsync(PaymentDto payment, string apiKey, string apiSecret);
}
