using AutoMapper;
using PaymentService.Models;
using Shared.PaymentService;

namespace PaymentService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<PaymentDto, Payment>();
    }
}
