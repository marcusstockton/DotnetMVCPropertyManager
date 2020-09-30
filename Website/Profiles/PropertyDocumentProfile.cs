using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
