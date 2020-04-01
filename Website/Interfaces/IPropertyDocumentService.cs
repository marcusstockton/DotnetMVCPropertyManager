using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Website.Models;

namespace Website.Interfaces
{
    public interface IPropertyDocumentService
    {
        Task<int> CreatePropertyDocumentsForProperty(Property property, List<IFormFile> documents);
        Task<List<DocumentType>> GetDocumentTypes();
    }
}
