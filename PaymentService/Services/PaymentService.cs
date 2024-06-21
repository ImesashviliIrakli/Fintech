using AutoMapper;
using PaymentService.Models;
using PaymentService.Repositories;
using Shared;
using Shared.PaymentService;

namespace PaymentService.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IMapper _mapper;

    public PaymentService(IPaymentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<OrderStatus> ProcessPaymentAsync(CreatePaymentDto payment)
    {
        // TODO: CHECK if order exists

        var newPayment = await _repository.CreatePaymentAsync(_mapper.Map<Payment>(payment));

        // TODO: Send to Services depending on card number
    }
}
