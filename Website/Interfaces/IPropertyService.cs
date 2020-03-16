using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Interfaces
{
    public interface IPropertyService
    {
        Task<List<Property>> GetPropertiesForPortfolio(Guid portfolioId);
        Task<Property> GetPropertyById(Guid propertyId);
    }
}
