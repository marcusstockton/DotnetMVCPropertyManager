using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
