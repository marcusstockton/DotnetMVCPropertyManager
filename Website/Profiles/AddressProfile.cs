using AutoMapper;
using Website.Models;
using Website.Models.DTOs;

namespace Website.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}