using AutoMapper;
using Website.Models;
using Website.Models.DTOs.Portfolios;

namespace Website.Profiles
{
    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<Portfolio, PortfolioDetailsDto>()
                .ForMember(x => x.NumberOfProperties, opt => opt.Ignore())
                .ForMember(x => x.GrossIncome, opt => opt.Ignore())
                .ForMember(x => x.TotalPropertyValue, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}