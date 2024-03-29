﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Extensions.Alerts;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.Documents;
using Website.Models.DTOs.Properties;

namespace Website.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyImageService _propertyImageService;
        private readonly IPropertyDocumentService _propertyDocumentService;
        private readonly IPortfolioService _portfolioService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertyController(IPropertyService propertyService, IPropertyImageService propertyImageService, IPropertyDocumentService propertyDocumentService, IPortfolioService portfolioService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _propertyService = propertyService;
            _propertyImageService = propertyImageService;
            _propertyDocumentService = propertyDocumentService;
            _portfolioService = portfolioService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid portfolioId)
        {
            var properties = await _propertyService.GetPropertiesForPortfolio(portfolioId);
            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid portfolioId, Guid propertyId)
        {
            var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
            var propertyDto = _mapper.Map<PropertyDetailDTO>(property);
            return View(propertyDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid portfolioId, Guid propertyId)
        {
            var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
            var propertyDto = _mapper.Map<PropertyDetailDTO>(property);
            return View(propertyDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyDetailDTO property, List<IFormFile> images, List<DocumentUploader> documents)
        {
            if (ModelState.IsValid)
            {
                var updatedProperty = _mapper.Map<Property>(property);
                var portfolio = _mapper.Map<Portfolio>(property.Portfolio);
                //if (images.Any())
                //{
                //    var imagesSaved = await _propertyImageService.CreateImagesForProperty(updatedProperty, images);
                //}
                //if (documents.Any())
                //{
                //    await _propertyDocumentService.CreatePropertyDocumentsForProperty(updatedProperty, documents);
                //}

                await _propertyService.UpdateProperty(updatedProperty);
                return RedirectToAction(nameof(Detail), new { portfolioId = property.Portfolio.Id, propertyId = property.Id })
                    .WithSuccess("Success", "Property Updated Sucessfully!");
            }
            return View(property).WithDanger("Error", "Some Errors Occured");
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid portfolioId)
        {
            var portfolio = await _portfolioService.GetPortfolioById(portfolioId);
            var property = new PropertyCreateView { Portfolio = portfolio };
            return View(property);
        }

        [HttpPost]
        [RequestSizeLimit(1_074_790_400)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyCreateView propertyCreateView)
        {
            if (ModelState.IsValid)
            {
                var property = _mapper.Map<Property>(propertyCreateView);
                var new_property = await _propertyService.CreateProperty(property);
                if (propertyCreateView.Images != null && propertyCreateView.Images.Any())
                {
                    try
                    {
                        await _propertyImageService.CreateImagesForProperty(new_property, propertyCreateView.Images);
                    }
                    catch (BadImageFormatException ex)
                    {
                        ModelState.AddModelError("Images", ex.Message);
                        return View(propertyCreateView);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Images", ex.Message);
                        return View(propertyCreateView);
                    }
                }
                if (propertyCreateView.Documents != null && propertyCreateView.Documents.Any())
                {
                    try
                    {
                        await _propertyDocumentService.CreatePropertyDocumentsForProperty(new_property, propertyCreateView.Documents);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Documents", ex.Message);
                        return View(propertyCreateView);
                    }
                }
                new_property.CreatedDate = DateTime.Now;
                new_property.Address.CreatedDate = DateTime.Now;
                await _propertyService.SaveAsync();
                return RedirectToAction("Details", "Portfolio", new { id = propertyCreateView.Portfolio.Id }).WithSuccess("Success", "Property Created successfully.");
            }

            return View(propertyCreateView).WithDanger("Error", "Please fix the errors");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid portfolioId, Guid propertyId)
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
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.DocumentTypes = await _propertyDocumentService.GetDocumentTypes(user);

            property.Documents.Add(new DocumentUploader());
            return PartialView("PropertyDocuments", property);
        }
    }
}