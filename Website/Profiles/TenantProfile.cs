using AutoMapper;
using Website.Models;
using Website.Models.DTOs.Tenants;

namespace Website.Profiles
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            CreateMap<Tenant, TenantDTO>().ReverseMap();
            CreateMap<Tenant, TenantCreateDTO>().ReverseMap();
        }
    }
}