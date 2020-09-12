using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Interfaces
{
    public interface IPropertyImageService
    {
        Task<int> CreateImagesForProperty(Property property, List<IFormFile> images);
        Task<string> FileToBase64String(string fileLocation);
    }
}