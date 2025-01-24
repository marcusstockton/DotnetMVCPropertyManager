using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Helpers;
using Website.Interfaces;
using Website.Models.DTOs.Portfolios;

namespace Website.Areas
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(IPortfolioService context, IMemoryCache memoryCache, ILogger<PortfolioController> logger)
        {
            _context = context;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [HttpGet("GetMyPortfolios")]
        public async Task<ActionResult<IList<PortfolioDetailsDto>>> GetMyPortfolios()
        {
            _logger.LogInformation($"{nameof(GetMyPortfolios)} getting my portfolios");

            var cacheKey = $"portfolio-{this.User.GetUserId()}";
            var cachedValue = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetSize(100);
                entry.SlidingExpiration = TimeSpan.FromMinutes(15);
                _logger.LogInformation("Cache miss, fetching data.");
                return await _context.GetMyPortfolios(this.User.GetUserId());
            });
            _logger.LogInformation($"{nameof(GetMyPortfolios)} complete.");
            return Ok(cachedValue);
        }
    }
}