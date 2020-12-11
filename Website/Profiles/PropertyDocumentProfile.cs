using AutoMapper;
using Website.Models;
using Website.Models.DTOs.PropertyDocuments;

namespace Website.Profiles
{
    public class PropertyDocumentProfile : Profile
    {
        public PropertyDocumentProfile()
        {
            CreateMap<PropertyDocument, PropertyDocumentDetailsDto>().ReverseMap();
        }
    }
}