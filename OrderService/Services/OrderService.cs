using AutoMapper;
using OrderService.Models;
using OrderService.Repositories;
using Shared.Dtos.Order;
using Shared.Dtos.Payment;
using Shared.Enums;
using Shared.Exceptions;

namespace OrderService.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;
    public OrderService(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task CompleteOrderAsync(PaymentStatusDto orderDto)
    {
        var check = await CheckCompletedOrdersAmountAsync(orderDto.CompanyId);

        if (!check)
            return;

        await _orderRepository.CompleteOrderAsync(orderDto.OrderId, (int)orderDto.OrderStatus);

        _logger.LogInformation($"Order with id:{orderDto.OrderId} completed");
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

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto)
    {
        var newOrder = await _orderRepository.CreateOrderAsync(_mapper.Map<Order>(orderDto));

        return _mapper.Map<OrderDto>(newOrder);
    }

    public async Task<OrderDto> GetOrderByIdAsync(int orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);

        if (order == null)
            throw new NotFoundException($"Order with id:{orderId} not found");

        return _mapper.Map<OrderDto>(order);
    }

    private async Task<bool> CheckCompletedOrdersAmountAsync(int companyId)
    {
        var orders = await _orderRepository.GetAllCompanyOrdersAsync(companyId);

        var totalCompleted = orders.Where(x => x.Status == (int)OrderStatus.Completed).Sum(x => x.Amount);

        if (totalCompleted > 10000)
        {
            _logger.LogWarning($"The total of all completed orders exceeds 10000$ for company:{companyId}");
            return false;
        }

        return true;
    }
}
