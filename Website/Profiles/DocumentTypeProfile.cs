using AutoMapper;
using Website.Models;
using Website.Models.DTOs.DocumentTypes;

namespace Website.Profiles
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<DocumentType, DocumentTypeCreateDto>().ReverseMap();
            CreateMap<DocumentType, DocumentTypeDetailDto>().ReverseMap();
        }
    }
}