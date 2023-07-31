using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Data;
using Website.Interfaces;
using Website.Models;

namespace Website.Services
{
    public class TenantService : ITenantService
    {
        private const string nationalityListCacheKey = "nationalityList";
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;
        private IMemoryCache _cache;

        public TenantService(ApplicationDbContext context, IWebHostEnvironment env, ILogger<TenantService> logger, IMemoryCache cache)
        {
            _context = context;
            _env = env;
            _logger = logger;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<Tenant> CreateTenant(Tenant tenant)
        {
            tenant.CreatedDate = DateTime.Now;
            await _context.Tenants.AddAsync(tenant);
            return tenant;
        }

        public async Task<string> CreateTenantImage(Guid tenantId, IFormFile file)
        {
            var path = Path.Combine(_env.WebRootPath, "TenantImages", tenantId.ToString());
            var filename = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(path, filename);
            var shortFilePath = filePath.Split(_env.WebRootPath).Last();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // Save stuff off
            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            return shortFilePath;
        }

        public async Task<bool> DeleteTenantImage(string fileLocation)
        {
            return await Task.Run(() =>
            {
                var path = Path.Combine(_env.WebRootPath, fileLocation.TrimStart(Path.DirectorySeparatorChar));
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            });
        }

        public async Task<Tenant> GetTenantByIdAsync(Guid tenantId)
        {
            return await _context.Tenants.FindAsync(tenantId);
        }


        public async Task<List<Nationality>> GetNationalitiesAsync()
        {
            _logger.LogInformation("Fetching list of nationalities");
            if (_cache.TryGetValue(nationalityListCacheKey, out IEnumerable<Nationality> nationalities))
            {
                _logger.LogInformation("Nationalities list found in cache.");
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (_cache.TryGetValue(nationalityListCacheKey, out nationalities))
                    {
                        _logger.LogInformation("Nationalities list found in cache.");
                    }
                    else
                    {
                        _logger.LogInformation("Nationalities List not found in cache. Fetching from database.");
                        nationalities = await _context.Nationalities.ToListAsync();

                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                                .SetPriority(CacheItemPriority.Normal)
                                .SetSize(1024);
                        _cache.Set(nationalityListCacheKey, nationalities, cacheEntryOptions);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
            return nationalities.ToList();
        }

        public async Task<Tenant> UpdateTenant(Tenant obj)
        {
            _context.Tenants.Update(obj);

            await SaveAsync();

            return await _context.Tenants
                .AsNoTracking()
                .Include(x => x.Property)
                .ThenInclude(x => x.Portfolio)
                .SingleOrDefaultAsync(t => t.Id == obj.Id);
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