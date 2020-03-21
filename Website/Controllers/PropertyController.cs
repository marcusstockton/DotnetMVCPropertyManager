using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs;

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
            return View( await _propertyService.GetPropertiesForPortfolio(portfolioId) );
        }

        [HttpGet]
        public async Task<IActionResult> GetPropertyById(Guid portfolioId, Guid propertyId)
        {
            return View(await _propertyService.GetPropertyById(portfolioId, propertyId));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProperty(Guid portfolioId, Guid propertyId)
        {
            return View( await _propertyService.GetPropertyById( portfolioId, propertyId ) );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProperty(Property property, List<IFormFile> images, List<IFormFile> documents)
        {
            if (ModelState.IsValid)
            {
                if (images.Any())
                {
                    var imagesSaved = await _propertyImageService.CreateImagesForProperty( property, images );
                }
                if (documents.Any())
                {
                    var documentsSaved = await _propertyDocumentService.CreatePropertyDocumentsForProperty( property, documents );
                }
                await _propertyService.UpdateProperty( property );
                return RedirectToAction( nameof( GetPropertyById ), new { portfolioId = property.Portfolio.Id, propertyId = property.Id } );
            }
            return View( property );
        }


        [HttpGet]
        public async Task<IActionResult> CreateProperty(Guid portfolioId)
        {
            // View with property, portfolio, images, address, documents
            var portfolio = await _portfolioService.GetPortfolioById( portfolioId );
            var property = new PropertyCreateView { Portfolio = portfolio};
            return View( property );
        }
        [HttpPost]
        public async Task<IActionResult> CreateProperty(PropertyCreateView propertyCreateView)
        {

            if (ModelState.IsValid)
            {
                var property = _mapper.Map<Property>( propertyCreateView );
                var new_property = await _propertyService.CreateProperty( property );
                if (propertyCreateView.Images != null && propertyCreateView.Images.Any())
                {
                    await _propertyImageService.CreateImagesForProperty( new_property, propertyCreateView.Images );
                }
                if (propertyCreateView.Documents != null && propertyCreateView.Documents.Any())
                {
                    await _propertyDocumentService.CreatePropertyDocumentsForProperty( new_property, propertyCreateView.Documents );
                }
                new_property.CreatedDate = DateTime.Now;
                new_property.Address.CreatedDate = DateTime.Now;
                await _propertyService.SaveAsync();
                return RedirectToAction( "Details", "Portfolio", new { id = propertyCreateView.Portfolio.Id} );
            }
           
            return View( propertyCreateView );
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDeleteProperty(Guid portfolioId, Guid propertyId)
        {
            var property = await _propertyService.GetPropertyById( portfolioId, propertyId );
            return View( property );
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty(Guid portfolioId, Guid propertyId)
        {
            await _propertyService.DeleteProperty( propertyId );
            await _propertyService.SaveAsync();
            return RedirectToAction( "Details", "Portfolio", new { id= portfolioId } );
        }
    }
}