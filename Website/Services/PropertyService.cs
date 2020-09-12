using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Data;
using Website.Interfaces;
using Website.Models;

namespace Website.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public PropertyService(ApplicationDbContext context, ILogger<PropertyService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Property>> GetPropertiesForPortfolio(Guid portfolioId)
        {
            return await _context.Properties.Include(x => x.Address)
                .Where(x => x.Portfolio.Id == portfolioId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Property> GetPropertyById(Guid portfolioId, Guid propertyId)
        {
            return await _context.Properties
                .Include(x => x.Portfolio)
                .Include(c => c.Address)
                .Include(x => x.Tenants)
                .Include(x => x.Images)
                .Include(x => x.Documents)
                .SingleOrDefaultAsync(x => x.Id == propertyId && x.Portfolio.Id == portfolioId);
        }

        public async Task<Property> CreateProperty(Property property)
        {
            property.CreatedDate = DateTime.Now;
            _context.Entry(property.Portfolio).State = EntityState.Unchanged;
            await _context.Properties.AddAsync(property);
            return property;
        }

        public async Task<Property> UpdateProperty(Property property)
        {
            property.UpdatedDate = DateTime.Now;
            _context.Entry(property.Portfolio).State = EntityState.Unchanged;
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task DeleteProperty(Guid propertyId)
        {
            var property = await _context.Properties
                .Include(x => x.Documents)
                .Include(x => x.Images)
                .Include(x => x.Address)
                .SingleAsync(x => x.Id == propertyId);
            _context.Properties.Remove(property);
        }

        public async Task<int> SaveAsync()
        {
            var added = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            var deleted = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);
            var updated = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            _logger.LogInformation($"Adding {added.Count()} entities");
            _logger.LogInformation($"Updating {updated.Count()} entities");
            _logger.LogInformation($"Deleting {deleted.Count()} entities");
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}