using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Interfaces;

namespace Website.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        public AdminController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _portfolioService.GetPortfolios());
        }
    }
}