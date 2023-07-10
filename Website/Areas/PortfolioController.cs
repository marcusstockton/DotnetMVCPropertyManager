using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(IPortfolioService context, ILogger<PortfolioController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetMyPortfolios"), ResponseCache(CacheProfileName = "Default60")]
        public async Task<ActionResult<IList<PortfolioDetailsDto>>> GetMyPortfolios()
        {
            _logger.LogInformation($"{nameof(GetMyPortfolios)} getting my portfolios");
            var sw = new Stopwatch();
            sw.Start();
            var portfolios = await _context.GetMyPortfolios(this.User.GetUserId());
            sw.Stop();
            _logger.LogInformation($"{nameof(GetMyPortfolios)} took {sw.ElapsedMilliseconds}ms to complete.");
            sw.Reset();
            return Ok(portfolios);
        }
    }
}