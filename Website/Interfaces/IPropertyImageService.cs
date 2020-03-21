using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Website.Models;

namespace Website.Interfaces
{
    public interface IPropertyImageService
    {
        Task<int> CreateImagesForProperty(Property property, List<IFormFile> images);
    }
}
