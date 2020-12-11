using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Interfaces
{
    public interface IPropertyImageService
    {
        Task<int> CreateImagesForProperty(Property property, List<IFormFile> images);

        Task<bool> CreateImageForProperty(Property property, IFormFile image);

        Task<string> FileToBase64String(string fileLocation);
    }
}