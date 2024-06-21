using PaymentService.Models;

namespace PaymentService.Repositories;

public interface IPaymentRepository
{
    Task<Payment> CreatePaymentAsync(Payment payment);
}
