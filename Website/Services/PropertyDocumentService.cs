using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Website.Data;
using Website.Interfaces;
using Website.Models;

namespace Website.Services
{
    public class PropertyDocumentService : IPropertyDocumentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public PropertyDocumentService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<int> CreatePropertyDocumentsForProperty(Property property, List<IFormFile> documents)
        {
            return 0;
        }
    }
}
