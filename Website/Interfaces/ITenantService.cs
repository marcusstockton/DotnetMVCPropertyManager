using Microsoft.AspNetCore.Http;
using System;
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

        Task<Tenant> UpdateTenant(Tenant obj);

        Task<int> SaveAsync();
    }
}