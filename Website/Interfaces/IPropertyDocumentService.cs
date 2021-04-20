using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;
using Website.Models.DTOs.Documents;
using Website.Models.DTOs.PropertyDocuments;

namespace Website.Interfaces
{
    public interface IPropertyDocumentService
    {
        Task<int> CreatePropertyDocumentsForProperty(Property property, List<DocumentUploader> documents);

        Task<bool> CreatePropertyDocumentForProperty(PropertyDocumentCreateDto propertyDoc);

        Task<List<DocumentType>> GetDocumentTypes(ApplicationUser user);
    }
}