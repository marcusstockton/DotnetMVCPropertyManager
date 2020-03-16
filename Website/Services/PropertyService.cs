using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Interfaces;
using Website.Models;

namespace Website.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;
        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Property>> GetPropertiesForPortfolio(Guid portfolioId)
        {
            return await _context.Properties.Where(x=>x.Portfolio.Id == portfolioId).ToListAsync();
        }

        public async Task<Property> GetPropertyById(Guid propertyId)
        {
            return await _context.Properties.FindAsync(propertyId);
        }

        public async Task<Property> CreateProperty(Property property)
        {
            await _context.AddAsync(property);
            await _context.SaveChangesAsync();
            return property;
        }
    }
}
