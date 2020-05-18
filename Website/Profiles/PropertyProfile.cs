using AutoMapper;
using Website.Models;
using Website.Models.DTOs;
using Website.Models.DTOs.Properties;

namespace Website.Profiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyCreateView>().ReverseMap()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Portfolio, opt => opt.MapFrom(src => src.Portfolio))
                .ForMember(dest => dest.Documents, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Property, PropertyDetailDTO>().ReverseMap()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
               
        }
    }
}