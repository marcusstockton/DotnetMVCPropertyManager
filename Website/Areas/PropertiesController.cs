using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.Models.DTOs.Properties;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly ILogger<PropertiesController> _logger;

        public PropertiesController(IPropertyService propertyService, ILogger<PropertiesController> logger)
        {
            _logger = logger;
            _propertyService = propertyService;
        }

        [HttpGet("GetPropertiesForPortfolio/{portfolioId}")]
        public async Task<ActionResult<IList<PropertyListDTO>>> GetPropertiesForPortfolio(Guid portfolioId)
        {
            _logger.LogInformation($"{nameof(GetPropertiesForPortfolio)} called");
            var propertiesForPortfolio = await _propertyService.GetPropertiesForPortfolio(portfolioId);
            _logger.LogInformation($"{nameof(GetPropertiesForPortfolio)} complete.");

            var properties = propertiesForPortfolio.Select(x => new PropertyListDTO
            {
                Id = x.Id,
                Address = new Models.DTOs.AddressDTO
                {
                    Id = x.Address.Id,
                    Line1 = x.Address.Line1,
                    Line2 = x.Address.Line2,
                    Line3 = x.Address.Line3,
                    City = x.Address.City,
                    CreatedDateTime = x.Address.CreatedDate.DateTime,
                    Latitude = x.Address.Latitude,
                    Longitude = x.Address.Longitude,
                    Postcode = x.Address.Postcode,
                    Town = x.Address.Town,
                    UpdatedDateTime = x.Address.UpdatedDate.GetValueOrDefault().DateTime
                },
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                AddressId = x.AddressId,
                Bathrooms = x.Bathrooms,
                MonthlyRentAmount = x.MonthlyRentAmount,
                NoOfRooms = x.NoOfRooms,
                PropertyValue = x.PropertyValue,
                PurchaseDate = x.PurchaseDate,
            }).ToList();

            return Ok(properties);
        }
    }
}