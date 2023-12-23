using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Website.Helpers;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.Portfolios;

namespace Website.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IMapper _mapper;
        private MapperConfiguration config;

        public AdminController(IPortfolioService portfolioService, IMapper mapper)
        {
            _portfolioService = portfolioService;
            _mapper = mapper;

            config = new MapperConfiguration(cfg =>
                cfg.CreateProjection<Portfolio, PortfolioAdminIndexDto>());
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CreatedDateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var portfolios = _portfolioService.GetPortfolios();

            if (!string.IsNullOrEmpty(searchString))
            {
                portfolios = portfolios.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    portfolios = portfolios.OrderByDescending(s => s.Name);
                    break;

                case "Date":
                    portfolios = portfolios.OrderBy(s => s.CreatedDate);
                    break;

                case "date_desc":
                    portfolios = portfolios.OrderByDescending(s => s.CreatedDate);
                    break;

                default:
                    portfolios = portfolios.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 5;
            var portList = portfolios.ProjectTo<PortfolioAdminIndexDto>(config);
            return View(await PaginatedList<PortfolioAdminIndexDto>.CreateAsync(portList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}