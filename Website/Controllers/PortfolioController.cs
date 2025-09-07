using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using Website.Extensions.Alerts;
using Website.Helpers;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.Portfolios;
using Website.Models.DTOs.Properties;

namespace Website.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _context;
        private readonly IMemoryCache _memoryCache;

        public PortfolioController(IPortfolioService context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        // GET: Portfolios
        public IActionResult Index()
        {
            return View();
        }

        // GET: Portfolios/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.GetPortfolioById(id.Value);
            if (portfolio == null)
            {
                return NotFound();
            }
            var portfolioDto = MapPortfolioToDetailsDto(portfolio);

            return View(portfolioDto);
        }

        // GET: Portfolios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Portfolios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,CreatedDate,UpdatedDate")] Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                await _context.CreatePortfolio(portfolio, username);
                _memoryCache.Remove($"portfolio-{this.User.GetUserId()}");
                return RedirectToAction(nameof(Index));
            }
            return View(portfolio).WithSuccess("Success", "Property Created Sucessfully");
        }

        // GET: Portfolios/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.GetPortfolioById(id.Value);
            if (portfolio == null)
            {
                return NotFound();
            }
            return View(portfolio);
        }

        // POST: Portfolios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Id,CreatedDate,UpdatedDate,OwnerId")] Portfolio portfolio)
        {
            if (id != portfolio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdatePortfolio(portfolio);
                    _memoryCache.Remove($"portfolio-{this.User.GetUserId()}");
                    if (this.User.IsInRole("Admin"))
                    {
                        _memoryCache.Remove($"portfolio-{portfolio.OwnerId}");
                        return RedirectToAction(nameof(Index), "Admin");
                    }
                    return RedirectToAction(nameof(Index)).WithSuccess("Success", "Portfolio Sucessfully Updated");
                }
                catch
                {
                    throw;
                }
            }
            return View(portfolio).WithWarning("Warning", "Invalid details.");
        }

        // GET: Portfolios/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.GetPortfolioById(id.Value);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }

        // POST: Portfolios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var portfolio = await _context.GetPortfolioById(id);
            if (await _context.DeletePortfolio(portfolio))
            {
                _memoryCache.Remove($"portfolio-{this.User.GetUserId()}");
                return RedirectToAction(nameof(Index)).WithSuccess("Success", "Portfolio Deleted");
            }
            return View(portfolio);
        }

        private PortfolioDetailsDto MapPortfolioToDetailsDto(Portfolio portfolio)
        {
            return new PortfolioDetailsDto
            {
                Id = portfolio.Id,
                CreatedDate = portfolio.CreatedDate,
                UpdatedDate = portfolio.UpdatedDate,
                Name = portfolio.Name,
                Owner = portfolio.Owner,
                Properties = portfolio.Properties?
                    .Select(p => new PropertyListDTO
                    {
                        Id = p.Id,
                        Description = p.Description, // Adjust if you have a Name property on Property
                        PropertyValue = p.PropertyValue,
                        MonthlyRentAmount = p.MonthlyRentAmount,
                        Address = new Models.DTOs.AddressDTO
                        {
                            Id = p.Address.Id,
                            Line1 = p.Address.Line1,
                            Line2 = p.Address.Line2,
                            Line3 = p.Address.Line3,
                            City = p.Address.City,
                            Postcode = p.Address.Postcode,
                            CreatedDateTime = p.Address.CreatedDate.DateTime,
                            UpdatedDateTime = p.Address.UpdatedDate.GetValueOrDefault().DateTime,
                            Latitude = p.Address.Latitude,
                            Longitude = p.Address.Longitude,
                            Town = p.Address.Town
                        },
                        AddressId = p.AddressId,
                        CreatedDate = p.CreatedDate,
                        UpdatedDate = p.UpdatedDate,
                        Bathrooms = p.Bathrooms,
                        NoOfRooms = p.NoOfRooms,
                        PurchaseDate = p.PurchaseDate,
                    }).ToList()
            };
        }
    }
}