using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;
using Website.Models.DTOs.Documents;

namespace Website.Interfaces
{
    public interface IPropertyDocumentService
    {
        Task<int> CreatePropertyDocumentsForProperty(Property property, List<DocumentUploader> documents);

        Task<List<DocumentType>> GetDocumentTypes(ApplicationUser user);
    }
}