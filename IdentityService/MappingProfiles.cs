using AutoMapper;
using IdentityService.Models;
using Shared.Dtos.Identity;

namespace IdentityService;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Company, CompanyDto>().ReverseMap();
    }
}
