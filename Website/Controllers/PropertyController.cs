using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Extensions.Alerts;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs;
using Website.Models.DTOs.Documents;
using Website.Models.DTOs.Properties;
using Website.Models.DTOs.Tenants;

namespace Website.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyImageService _propertyImageService;
        private readonly IPropertyDocumentService _propertyDocumentService;
        private readonly IPortfolioService _portfolioService;
        private readonly IMapper _mapper;

        public PropertyController(IPropertyService propertyService, IPropertyImageService propertyImageService, IPropertyDocumentService propertyDocumentService, IPortfolioService portfolioService, IMapper mapper)
        {
            _propertyService = propertyService;
            _propertyImageService = propertyImageService;
            _propertyDocumentService = propertyDocumentService;
            _portfolioService = portfolioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid portfolioId)
        {
            var properties = await _propertyService.GetPropertiesForPortfolio(portfolioId);
            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> GetPropertyById(Guid portfolioId, Guid propertyId)
        {
            var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
            var propertyDto = _mapper.Map<PropertyDetailDTO>(property);
            return View(propertyDto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProperty(Guid portfolioId, Guid propertyId)
        {
            var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProperty(Property property, List<IFormFile> images, List<DocumentUploader> documents)
        {
            if (ModelState.IsValid)
            {
                if (images.Any())
                {
                    var imagesSaved = await _propertyImageService.CreateImagesForProperty(property, images);
                }
                if (documents.Any())
                {
                    await _propertyDocumentService.CreatePropertyDocumentsForProperty(property, documents);
                }
                await _propertyService.UpdateProperty(property);
                return RedirectToAction(nameof(Index), new { portfolioId = property.Portfolio.Id, propertyId = property.Id })
                    .WithSuccess("Success", "Property Updated Sucessfully!");
            }
            return View(property).WithDanger("Error", "Some Errors Occured");
        }

        [HttpGet]
        public async Task<IActionResult> CreateProperty(Guid portfolioId)
        {
            var portfolio = await _portfolioService.GetPortfolioById(portfolioId);
            var property = new PropertyCreateView { Portfolio = portfolio };
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProperty(PropertyCreateView propertyCreateView)
        {
            if (ModelState.IsValid)
            {
                var property = _mapper.Map<Property>(propertyCreateView);
                var new_property = await _propertyService.CreateProperty(property);
                if (propertyCreateView.Images != null && propertyCreateView.Images.Any())
                {
                    await _propertyImageService.CreateImagesForProperty(new_property, propertyCreateView.Images);
                }
                if (propertyCreateView.Documents != null && propertyCreateView.Documents.Any())
                {
                    await _propertyDocumentService.CreatePropertyDocumentsForProperty(new_property, propertyCreateView.Documents);
                }
                new_property.CreatedDate = DateTime.Now;
                new_property.Address.CreatedDate = DateTime.Now;
                await _propertyService.SaveAsync();
                return RedirectToAction("Details", "Portfolio", new { id = propertyCreateView.Portfolio.Id }).WithSuccess("Success", "Property Created successfully.");
            }

            return View(propertyCreateView).WithDanger("Error", "Please fix the errors");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDeleteProperty(Guid portfolioId, Guid propertyId)
        {
            var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProperty(Guid portfolioId, Guid propertyId)
        {
            await _propertyService.DeleteProperty(propertyId);
            await _propertyService.SaveAsync();
            return RedirectToAction("Details", "Portfolio", new { id = portfolioId }).WithSuccess("Deleted", "Property Deleted successfully.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPropertyDocument([Bind(include: "Documents")] PropertyCreateView property)
        {
            ViewBag.DocumentTypes = new SelectList(await _propertyDocumentService.GetDocumentTypes(), "Id", "Description");

            property.Documents.Add(new DocumentUploader());
            return PartialView("PropertyDocuments", property);
        }
    }
}