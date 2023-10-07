using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Interfaces
{
    public interface ITenantService
    {
        Task<Tenant> CreateTenant(Tenant tenant);

        Task<string> CreateTenantImage(Guid tenantId, IFormFile file);

        Task<bool> DeleteTenantImage(string fileLocation);

        Task<Tenant> GetTenantByIdAsync(Guid tenantId);

        Task<List<Tenant>> GetTenantsForPropertyId(Guid propertyId);

        Task<Tenant> UpdateTenant(Tenant obj);
        Task<List<Nationality>> GetNationalitiesAsync();

        Task<int> SaveAsync();
    }
}