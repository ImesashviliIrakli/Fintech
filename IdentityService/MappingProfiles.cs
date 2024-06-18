using AutoMapper;
using IdentityService.Models;
using Shared.IdentityService;

namespace IdentityService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Company, CompanyDto>().ReverseMap();
    }
}
