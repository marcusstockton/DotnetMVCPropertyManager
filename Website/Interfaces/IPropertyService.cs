using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Interfaces
{
    public interface IPropertyService
    {
        Task<List<Property>> GetPropertiesForPortfolio(Guid portfolioId);

        Task<Property> GetPropertyById(Guid portfolioId, Guid propertyId);

        Task<Property> CreateProperty(Property property);

        Task<Property> UpdateProperty(Property property);

        Task DeleteProperty(Guid propertyId);

        Task<int> SaveAsync();
    }
}