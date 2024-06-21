using AutoMapper;
using OrderService.Models;
using OrderService.Repositories;
using Shared;
using Shared.Exceptions;
using Shared.OrderService;

namespace OrderService.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    public async Task<ComputeOrderDto> ComputeCompanyOrdersAsync(int companyId)
    {
        var orders = await _orderRepository.GetAllCompanyOrdersAsync(companyId);

        var result = new ComputeOrderDto
        {
            CompanyId = companyId,
            TotalAmount = orders.Sum(x => x.Amount)
        };

        return result;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto order)
    {
        var newOrder = await _orderRepository.CreateOrderAsync(_mapper.Map<Order>(order));

        return _mapper.Map<OrderDto>(newOrder);
    }

    private async Task CheckCompletedOrdersAmount(int companyId)
    {
        var orders = await _orderRepository.GetAllCompanyOrdersAsync(companyId);

        var totalCompleted = orders.Where(x => x.Status == (int)OrderStatus.Completed).Sum(x => x.Amount);

        if (totalCompleted > 10000)
            throw new ComputeException($"The total of all completed orders exceeds 10000$ for company:{companyId}");
    }
}
