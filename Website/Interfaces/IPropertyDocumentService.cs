using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Interfaces
{
    public interface IPropertyDocumentService
    {
        Task<int> CreatePropertyDocumentsForProperty(Property property, List<IFormFile> documents);

        Task<List<DocumentType>> GetDocumentTypes();
    }
}