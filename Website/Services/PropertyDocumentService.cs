﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Website.Data;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.Documents;

namespace Website.Services
{
    public class PropertyDocumentService : IPropertyDocumentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertyDocumentService(ApplicationDbContext context, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public async Task<bool> CreatePropertyDocumentForProperty(Guid propertyId, IFormFile file, Guid documentTypeId)
        {
            var result = false;
            var contentRootPath = _env.ContentRootPath;
            var uploads = Path.Combine(_env.WebRootPath, "PropertyDocuments", propertyId.ToString());
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            if (file.Length > 0)
            {
                // Upload the file if less than 2 MB
                if (file.Length < 2097152)
                {
                    var filePath = Path.Combine(uploads, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        await _context.AddAsync(new PropertyDocument
                        {
                            FileName = file.FileName,
                            FilePath = filePath,
                            CreatedDate = DateTime.Now,
                            DocumentTypeId = documentTypeId,
                            FileType = Path.GetExtension(file.FileName),
                            PropertyId = propertyId
                        });
                    }
                    await _context.SaveChangesAsync();
                    result = true;
                }
                else
                {
                    throw new ImageFormatLimitationException("The file is too large");
                }
            }
            return result;
        }

        public async Task<int> CreatePropertyDocumentsForProperty(Property property, List<DocumentUploader> documents)
        {
            var counter = 0;
            var uploads = Path.Combine(_env.WebRootPath, "PropertyDocuments", property.Id.ToString());
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            foreach (var file in documents)
            {
                if (file.Document.Length > 0)
                {
                    if (file.Document.Length < 2000000)
                    {
                        var filePath = Path.Combine(uploads, file.Document.FileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.Document.CopyToAsync(fileStream);
                            counter++;
                            await _context.AddAsync(new PropertyDocument
                            {
                                FileName = file.Document.FileName,
                                FilePath = filePath,
                                CreatedDate = DateTime.Now,
                                DocumentTypeId = file.DocumentType.Id,
                                ExpirationDate = file.ExpiryDate,
                                FileType = Path.GetExtension(file.Document.FileName),
                                Property = property
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"The file is too large at {Math.Round((file.Document.Length / 1024f) / 1024, 2)} MBs.");
                }
            }
            return counter;
        }

        public async Task<List<DocumentType>> GetDocumentTypes(ApplicationUser user)
        {
            if(await _userManager.IsInRoleAsync(user, "Owner"))
            {
                return await _context.DocumentTypes.Where(x=>x.Owner == user || x.Owner == null).ToListAsync();
            }
            return await _context.DocumentTypes.ToListAsync();
        }
    }
}