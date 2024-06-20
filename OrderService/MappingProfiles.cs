﻿using AutoMapper;
using OrderService.Models;
using Shared.OrderService;

namespace OrderService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<OrderDto, Order>().ReverseMap();
        CreateMap<CreateOrderDto, Order>();
    }
}
