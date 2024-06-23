using AutoMapper;
using PaymentService.Models;
using Shared.Dtos.Payment;

namespace PaymentService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<PaymentDto, Payment>();
    }
}
