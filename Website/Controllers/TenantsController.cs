using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
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
        private readonly ITenantService _tenantService;
        private readonly IPropertyService _propertyService;

        public TenantsController(ITenantService tenantService, IPropertyService propertyService)
        {
            _tenantService = tenantService;
            _propertyService = propertyService;
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _tenantService.GetTenantByIdAsync(id.Value);

            if (tenant == null)
            {
                return NotFound();
            }

            var dto = MapTenantToDTO(tenant);
            return PartialView("_tenantDetail", dto);
        }

        // GET: Tenants/Create
        public async Task<IActionResult> Create(Guid portfolioId, Guid propertyId)
        {
            var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
            var tenant = new Tenant { Property = property };
            var nationalities = await _tenantService.GetNationalitiesAsync();
            ViewBag.Nationalities = nationalities.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            var dto = MapTenantToCreateDTO(tenant);
            return PartialView("_TenantCreate", dto);
        }

        // POST: Tenants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid portfolioId, Guid propertyId, [Bind("FirstName,LastName,PhoneNumber,EmailAddress,JobTitle,NationalityId,TenancyStartDate,TenancyEndDate,TenantImage,Id,CreatedDate,UpdatedDate,IsSmoker,HasPets")] TenantCreateDTO tenantDto)
        {
            if (ModelState.IsValid)
            {
                var newTenant = MapCreateDTOToTenant(tenantDto);
                var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
                newTenant.Property = property;
                var o = await _tenantService.CreateTenant(newTenant);
                if (tenantDto.TenantImage != null)
                {
                    o.TenantImage = await _tenantService.CreateTenantImage(o.Id, tenantDto.TenantImage);
                }

                await _tenantService.SaveAsync();
                return Ok(o);
            }
            return BadRequest(tenantDto);
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
            var viewData = MapTenantToDTO(tenant);

            ViewBag.Nationalities = nationalities.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == viewData.NationalityId }).ToList();
            return View(viewData);
        }

        // POST: Tenants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FirstName,LastName,PhoneNumber,JobTitle,NationalityId,DateOfBirth,TenancyStartDate,TenancyEndDate,TenantImage,Id,CreatedDate,UpdatedDate,IsSmoker,HasPets,EmailAddress")] Tenant tenant, IFormFile profilePic)
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

        // Manual mapping helpers

        private TenantDTO MapTenantToDTO(Tenant tenant)
        {
            return new TenantDTO
            {
                Id = tenant.Id,
                CreatedDate = tenant.CreatedDate,
                UpdatedDate = tenant.UpdatedDate,
                FirstName = tenant.FirstName,
                LastName = tenant.LastName,
                PhoneNumber = tenant.PhoneNumber,
                EmailAddress = string.IsNullOrEmpty(tenant.EmailAddress) ? "" : tenant.EmailAddress,
                JobTitle = tenant.JobTitle,
                NationalityId = tenant.Nationality?.Id ?? 0,
                Nationality = tenant.Nationality,
                DateOfBirth = tenant.DateOfBirth,
                TenancyStartDate = tenant.TenancyStartDate,
                TenancyEndDate = tenant.TenancyEndDate,
                IsSmoker = tenant.IsSmoker,
                HasPets = tenant.HasPets,
                Property = tenant.Property,
                TenantImage = tenant.TenantImage,
                Notes = tenant.Notes
            };
        }

        private TenantCreateDTO MapTenantToCreateDTO(Tenant tenant)
        {
            return new TenantCreateDTO
            {
                Id = tenant.Id,
                CreatedDate = tenant.CreatedDate,
                UpdatedDate = (DateTimeOffset)tenant.UpdatedDate,
                FirstName = tenant.FirstName,
                LastName = tenant.LastName,
                PhoneNumber = tenant.PhoneNumber,
                EmailAdresss = tenant.EmailAddress,
                JobTitle = tenant.JobTitle,
                NationalityId = tenant.Nationality?.Id ?? 0,
                DateOfBirth = tenant.DateOfBirth,
                TenancyStartDate = tenant.TenancyStartDate,
                TenancyEndDate = tenant.TenancyEndDate,
                IsSmoker = tenant.IsSmoker,
                HasPets = tenant.HasPets,
                Property = tenant.Property,
                Notes = tenant.Notes
            };
        }

        private Tenant MapCreateDTOToTenant(TenantCreateDTO dto)
        {
            return new Tenant
            {
                Id = dto.Id,
                CreatedDate = dto.CreatedDate,
                UpdatedDate = dto.UpdatedDate,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                EmailAddress = dto.EmailAdresss,
                JobTitle = dto.JobTitle,
                Nationality = dto.NationalityId != 0 ? new Nationality { Id = dto.NationalityId } : null,
                DateOfBirth = dto.DateOfBirth?.DateTime,
                TenancyStartDate = dto.TenancyStartDate.DateTime,
                TenancyEndDate = dto.TenancyEndDate?.DateTime,
                IsSmoker = dto.IsSmoker,
                HasPets = dto.HasPets,
                Property = dto.Property,
                Notes = dto.Notes
            };
        }
    }
}