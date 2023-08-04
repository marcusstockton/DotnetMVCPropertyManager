using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Helpers;
using Website.Interfaces;
using Website.Models.DTOs.Portfolios;
using Website.Models.DTOs.Properties;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly ILogger<PropertiesController> _logger;
        private readonly IMapper _mapper;
        public PropertiesController(IPropertyService propertyService, ILogger<PropertiesController> logger, IMapper mapper)
        {
            _logger = logger;
            _propertyService = propertyService;
            _mapper = mapper;
        }

        [HttpGet("GetPropertiesForPortfolio/{portfolioId}")]
        public async Task<ActionResult<IList<PropertyListDTO>>> GetPropertiesForPortfolio(Guid portfolioId)
        {
            _logger.LogInformation($"{nameof(GetPropertiesForPortfolio)} called");
            var propertiesForPortfolio = await _propertyService.GetPropertiesForPortfolio(portfolioId);
            _logger.LogInformation($"{nameof(GetPropertiesForPortfolio)} complete.");
            var properties = _mapper.Map<List<PropertyListDTO>>(propertiesForPortfolio);
            return Ok(properties);
        }
    }
}
