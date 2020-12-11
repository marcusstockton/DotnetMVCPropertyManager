using AutoMapper;
using Website.Models;
using Website.Models.DTOs.PropertyImage;

namespace Website.Profiles
{
    public class PropertyImageProfile : Profile
    {
        public PropertyImageProfile()
        {
            CreateMap<PropertyImage, PropertyImageCreateDto>().ReverseMap();
        }
    }
}