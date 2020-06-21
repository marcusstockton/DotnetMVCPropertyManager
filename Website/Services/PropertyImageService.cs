using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Website.Data;
using Website.Interfaces;
using Website.Models;

namespace Website.Services
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private static string IMAGEFOLDER = "PropertyImages";

        public PropertyImageService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<int> CreateImagesForProperty(Property property, List<IFormFile> images)
        {
            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    // Upload the file if less than 2 MB
                    if (image.Length < 2000000)
                    {
                        var path = Path.Combine(_env.WebRootPath, IMAGEFOLDER, property.Id.ToString());
                        var filename = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(path, filename);
                        var shortFilePath = filePath.Split(_env.WebRootPath).Last();
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        // Save stuff off
                        using (var stream = File.Create(filePath))
                        {
                            await image.CopyToAsync(stream);
                            _context.PropertyImages.Add(
                                new PropertyImage
                                {
                                    CreatedDate = DateTime.Now,
                                    FileName = filename,
                                    FilePath = shortFilePath,
                                    FileType = Path.GetExtension(filePath),
                                    Property = property,
                                });
                        }
                    }
                    else
                    {
                        throw new BadImageFormatException($"The file is too large at {Math.Round((image.Length / 1024f) / 1024, 2)} MBs.", image.FileName);
                    }
                }
                //return (await _context.SaveChangesAsync());
            }
            return 1;
        }
    }
}