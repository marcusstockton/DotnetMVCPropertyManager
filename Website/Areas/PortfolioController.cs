using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public PortfolioController(IPortfolioService context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetMyPortfolios")]
        public async Task<ActionResult<IList<PortfolioDetailsDto>>> GetMyPortfolios()
        {
            var portfolios = await _context.GetMyPortfolios(this.User.GetUserId());
            var portfolioList = _mapper.Map<IList<PortfolioDetailsDto>>(portfolios);
            return Ok(portfolioList);
        }
    }
}