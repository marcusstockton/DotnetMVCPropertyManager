using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Extensions.Alerts;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.Tenants;

namespace Website.Controllers
{
    public class TenantsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly ITenantService _tenantService;

        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public TenantsController(ITenantService tenantService, IPropertyService propertyService, IMapper mapper)
        {
            _tenantService = tenantService;
            _propertyService = propertyService;
            _mapper = mapper;
        }

        // GET: Tenants
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Tenants.ToListAsync());
        //}

        //// GET: Tenants/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tenant = await _context.Tenants
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (tenant == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tenant);
        //}

        // GET: Tenants/Create
        public async Task<IActionResult> Create(Guid portfolioId, Guid propertyId)
        {
            var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
            var tenant = new Tenant { Property = property };
            var tenantDto = _mapper.Map<TenantCreateDTO>(tenant);
            var nationalities = await _tenantService.GetNationalitiesAsync();
            ViewBag.Nationalities = nationalities.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name}).ToList();
            return PartialView("_TenantCreate", tenantDto);
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid portfolioId, Guid propertyId, [Bind("FirstName,LastName,PhoneNumber,JobTitle,Nationality,TenancyStartDate,TenancyEndDate,TenantImage,Id,CreatedDate,UpdatedDate")] TenantCreateDTO tenant)
        {
            if (ModelState.IsValid)
            {
                var new_tenant = _mapper.Map<Tenant>(tenant);
                var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
                var o = await _tenantService.CreateTenant(new_tenant);
                o.Property = property;
                if (tenant.TenantImage != null)
                {
                    o.TenantImage = await _tenantService.CreateTenantImage(o.Id, tenant.TenantImage);
                }

                await _tenantService.SaveAsync();
                return Ok(o);
            }
            return BadRequest(tenant);
        }

        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _tenantService.GetTenantByIdAsync(id.Value);
            var nationalities = await _tenantService.GetNationalitiesAsync();
            if (tenant == null)
            {
                return NotFound();
            }
            var viewData = _mapper.Map<TenantDTO>(tenant);

            ViewBag.Nationalities = nationalities.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == viewData.NationalityId }).ToList();            
            return View(viewData);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FirstName,LastName,PhoneNumber,JobTitle,Nationality,TenancyStartDate,TenancyEndDate,TenantImage,Id,CreatedDate,UpdatedDate")] Tenant tenant, IFormFile profilePic)
        {
            if (id != tenant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (profilePic != null)
                {
                    // Uploaded a new image, delete the existing...
                    await _tenantService.DeleteTenantImage(tenant.TenantImage);
                    tenant.TenantImage = await _tenantService.CreateTenantImage(tenant.Id, profilePic);
                }
                var result = await _tenantService.UpdateTenant(tenant);

                var routeValues = new RouteValueDictionary {
                  { "portfolioId", result.Property.Portfolio.Id },
                  { "propertyId", result.Property.Id }
                };
                return RedirectToAction("Detail", "Property", routeValues, "#nav-tenant")
                    .WithSuccess("Success", "Tenant sucessfully updated");
            }
            return View(tenant);
        }

        //// GET: Tenants/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tenant = await _context.Tenants
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (tenant == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tenant);
        //}

        //// POST: Tenants/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var tenant = await _context.Tenants.FindAsync(id);
        //    _context.Tenants.Remove(tenant);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool TenantExists(Guid id)
        //{
        //    return _context.Tenants.Any(e => e.Id == id);
        //}
    }
}