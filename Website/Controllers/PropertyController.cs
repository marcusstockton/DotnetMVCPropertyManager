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
using Website.Models.DTOs;
using Website.Models.DTOs.Documents;
using Website.Models.DTOs.Portfolios;
using Website.Models.DTOs.Properties;
using Website.Models.DTOs.PropertyDocuments;
using Website.Models.DTOs.Tenants;

namespace Website.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyImageService _propertyImageService;
        private readonly IPropertyDocumentService _propertyDocumentService;
        private readonly IPortfolioService _portfolioService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertyController(IPropertyService propertyService, IPropertyImageService propertyImageService, IPropertyDocumentService propertyDocumentService, IPortfolioService portfolioService, UserManager<ApplicationUser> userManager)
        {
            _propertyService = propertyService;
            _propertyImageService = propertyImageService;
            _propertyDocumentService = propertyDocumentService;
            _portfolioService = portfolioService;
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
            //var propertyDto = _mapper.Map<PropertyDetailDTO>(property);
            var propertyDto = new PropertyDetailDTO
            {
                Id = property.Id,
                Address = new AddressDTO
                {
                    Id = property.Address.Id,
                    Line1 = property.Address.Line1,
                    Line2 = property.Address.Line2,
                    Line3 = property.Address.Line3,
                    City = property.Address.City,
                    CreatedDateTime = property.Address.CreatedDate.DateTime,
                    Latitude = property.Address.Latitude,
                    Longitude = property.Address.Longitude,
                    Postcode = property.Address.Postcode,
                    Town = property.Address.Town,
                    UpdatedDateTime = property.Address.UpdatedDate.GetValueOrDefault().DateTime
                },
                Description = property.Description,
                CreatedDate = property.CreatedDate,
                UpdatedDate = property.UpdatedDate,
                AddressId = property.AddressId,
                Bathrooms = property.Bathrooms,
                MonthlyRentAmount = property.MonthlyRentAmount,
                NoOfRooms = property.NoOfRooms,
                PropertyValue = property.PropertyValue,
                PurchaseDate = property.PurchaseDate,
                Portfolio = new PortfolioDetailsDto
                {
                    Id = property.Portfolio.Id,
                    Name = property.Portfolio.Name
                },

                Images = property.Images?.Select(pi => new PropertyImage
                {
                    Id = pi.Id,
                    Description = pi.Description,
                    FileName = pi.FileName,
                    FilePath = pi.FilePath,
                    FileType = pi.FileType,
                    CreatedDate = pi.CreatedDate,
                    UpdatedDate = pi.UpdatedDate
                }).ToList(),
                Tenants = property.Tenants?.Select(t => new TenantDTO
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    IsSmoker = t.IsSmoker,
                    HasPets = t.HasPets,
                    EmailAddress = t.EmailAddress,
                    DateOfBirth = t.DateOfBirth,
                    JobTitle = t.JobTitle,
                }).ToList(),
                Documents = property.Documents?.Select(pd => new PropertyDocumentDetailsDto
                {
                    Id = pd.Id,
                    FileName = pd.FileName,
                    FilePath = pd.FilePath,
                    FileType = pd.FileType,
                    DocumentTypeId = pd.DocumentTypeId,
                    DocumentType = pd.DocumentType != null ? new DocumentType
                    {
                        Id = pd.DocumentType.Id,
                        Description = pd.DocumentType.Description
                    } : null,
                    Expires = pd.Expires,
                    ActiveFrom = pd.ActiveFrom,
                    ExpirationDate = pd.ExpirationDate,
                    CreatedDate = pd.CreatedDate,
                    UpdatedDate = pd.UpdatedDate
                }).ToList(),
            };
            return View(propertyDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid portfolioId, Guid propertyId)
        {
            var property = await _propertyService.GetPropertyById(portfolioId, propertyId);
            //var propertyDto = _mapper.Map<PropertyDetailDTO>(property);

            var propertyDto = new PropertyDetailDTO
            {
                Id = property.Id,
                Address = new AddressDTO
                {
                    Id = property.Address.Id,
                    Line1 = property.Address.Line1,
                    Line2 = property.Address.Line2,
                    Line3 = property.Address.Line3,
                    City = property.Address.City,
                    CreatedDateTime = property.Address.CreatedDate.DateTime,
                    Latitude = property.Address.Latitude,
                    Longitude = property.Address.Longitude,
                    Postcode = property.Address.Postcode,
                    Town = property.Address.Town,
                    UpdatedDateTime = property.Address.UpdatedDate.GetValueOrDefault().DateTime
                },
                Description = property.Description,
                CreatedDate = property.CreatedDate,
                UpdatedDate = property.UpdatedDate,
                AddressId = property.AddressId,
                Bathrooms = property.Bathrooms,
                MonthlyRentAmount = property.MonthlyRentAmount,
                NoOfRooms = property.NoOfRooms,
                PropertyValue = property.PropertyValue,
                PurchaseDate = property.PurchaseDate,
                Portfolio = new PortfolioDetailsDto
                {
                    Id = property.Portfolio.Id,
                    Name = property.Portfolio.Name
                },
                Images = property.Images?.Select(pi => new PropertyImage
                {
                    Id = pi.Id,
                    Description = pi.Description,
                    FileName = pi.FileName,
                    FilePath = pi.FilePath,
                    FileType = pi.FileType,
                    CreatedDate = pi.CreatedDate,
                    UpdatedDate = pi.UpdatedDate
                }).ToList(),
                Documents = property.Documents?.Select(pd => new PropertyDocumentDetailsDto
                {
                    Id = pd.Id,
                    FileName = pd.FileName,
                    FilePath = pd.FilePath,
                    FileType = pd.FileType,
                    DocumentTypeId = pd.DocumentTypeId,
                    DocumentType = pd.DocumentType != null ? new DocumentType
                    {
                        Id = pd.DocumentType.Id,
                        Description = pd.DocumentType.Description
                    } : null,
                    Expires = pd.Expires,
                    ActiveFrom = pd.ActiveFrom,
                    ExpirationDate = pd.ExpirationDate,
                    CreatedDate = pd.CreatedDate,
                    UpdatedDate = pd.UpdatedDate
                }).ToList()
            };

            return View(propertyDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyDetailDTO property, List<IFormFile> images, List<DocumentUploader> documents)
        {
            if (ModelState.IsValid)
            {
                //var updatedProperty = _mapper.Map<Property>(property);
                var updatedProperty = new Property
                {
                    Id = property.Id,
                    AddressId = property.AddressId,
                    Address = new Address
                    {
                        Id = property.Address.Id,
                        Line1 = property.Address.Line1,
                        Line2 = property.Address.Line2,
                        Line3 = property.Address.Line3,
                        City = property.Address.City,
                        CreatedDate = property.Address.CreatedDateTime,
                        Latitude = property.Address.Latitude,
                        Longitude = property.Address.Longitude,
                        Postcode = property.Address.Postcode,
                        Town = property.Address.Town,
                        UpdatedDate = DateTime.Now
                    },
                    Description = property.Description,
                    CreatedDate = property.CreatedDate,
                    UpdatedDate = DateTime.Now,
                    Bathrooms = property.Bathrooms,
                    MonthlyRentAmount = property.MonthlyRentAmount,
                    NoOfRooms = property.NoOfRooms,
                    PropertyValue = property.PropertyValue,
                    PurchaseDate = property.PurchaseDate,
                    Images = property.Images?.Select(pi => new PropertyImage
                    {
                        Id = pi.Id,
                        Description = pi.Description,
                        FileName = pi.FileName,
                        FilePath = pi.FilePath,
                        FileType = pi.FileType,
                        CreatedDate = pi.CreatedDate,
                        UpdatedDate = pi.UpdatedDate
                    }).ToList(),
                    Documents = property.Documents?.Select(pd => new PropertyDocument
                    {
                        Id = pd.Id,
                        FileName = pd.FileName,
                        FilePath = pd.FilePath,
                        FileType = pd.FileType,
                        DocumentTypeId = pd.DocumentTypeId,
                        Expires = pd.Expires,
                        ActiveFrom = pd.ActiveFrom,
                        ExpirationDate = pd.ExpirationDate,
                        CreatedDate = pd.CreatedDate,
                        UpdatedDate = pd.UpdatedDate
                    }).ToList(),
                    Portfolio = new Portfolio
                    {
                        Id = property.Portfolio.Id,
                        Name = property.Portfolio.Name
                    },
                    Tenants = property.Tenants?.Select(t => new Tenant
                    {
                        Id = t.Id,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        IsSmoker = t.IsSmoker,
                        HasPets = t.HasPets,
                        EmailAddress = t.EmailAddress,
                        DateOfBirth = t.DateOfBirth,
                        JobTitle = t.JobTitle,
                        Nationality = t.Nationality,
                        Notes = t.Notes,
                        TenancyStartDate = t.TenancyStartDate,
                        TenancyEndDate = t.TenancyEndDate,
                        TenantImage = t.TenantImage,
                        PhoneNumber = t.PhoneNumber,
                        CreatedDate = t.CreatedDate,
                        UpdatedDate = t.UpdatedDate
                    }).ToList()
                };

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
                //var property = _mapper.Map<Property>(propertyCreateView);
                var property = new Property
                {
                    Id = Guid.NewGuid(),
                    Address = new Address
                    {
                        Id = Guid.NewGuid(),
                        Line1 = propertyCreateView.Address.Line1,
                        Line2 = propertyCreateView.Address.Line2,
                        Line3 = propertyCreateView.Address.Line3,
                        City = propertyCreateView.Address.City,
                        CreatedDate = DateTime.Now,
                        Latitude = propertyCreateView.Address.Latitude,
                        Longitude = propertyCreateView.Address.Longitude,
                        Postcode = propertyCreateView.Address.Postcode,
                        Town = propertyCreateView.Address.Town
                    },
                    Description = propertyCreateView.Description,
                    CreatedDate = DateTime.Now,
                    Bathrooms = propertyCreateView.Bathrooms,
                    MonthlyRentAmount = propertyCreateView.MonthlyRentAmount,
                    NoOfRooms = propertyCreateView.NoOfRooms,
                    PropertyValue = propertyCreateView.PropertyValue,
                    PurchaseDate = propertyCreateView.PurchaseDate ?? DateTime.Now,
                    Portfolio = new Portfolio
                    {
                        Id = propertyCreateView.Portfolio.Id,
                        Name = propertyCreateView.Portfolio.Name
                    },
                    Tenants = new List<Tenant> { }
                };

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

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ModelState.AddModelError("SearchTerm", "Please enter a search term.");
                return View();
            }

            var results = await _propertyService.PropertyFullTextSearchWithRank(searchTerm);
            var resultsDto = results.Select(property => new PropertyDetailDTO
            {
                Id = property.Id,
                Description = property.Description,
                Address = new AddressDTO
                {
                    Line1 = property.Address.Line1,
                    Line2 = property.Address.Line2,
                    Line3 = property.Address.Line3,
                    City = property.Address.City,
                    Postcode = property.Address.Postcode,
                    Town = property.Address.Town
                },
                MonthlyRentAmount = property.MonthlyRentAmount,
                NoOfRooms = property.NoOfRooms,
                PropertyValue = property.PropertyValue,
                PurchaseDate = property.PurchaseDate,
                AddressId = property.AddressId,
                Bathrooms = property.Bathrooms,
                CreatedDate = property.CreatedDate,
                Documents = property.Documents?.Select(pd => new PropertyDocumentDetailsDto
                {
                    Id = pd.Id,
                    FileName = pd.FileName,
                    FilePath = pd.FilePath,
                    FileType = pd.FileType,
                    DocumentTypeId = pd.DocumentTypeId,
                    DocumentType = pd.DocumentType != null ? new DocumentType
                    {
                        Id = pd.DocumentType.Id,
                        Description = pd.DocumentType.Description
                    } : null,
                    Expires = pd.Expires,
                    ActiveFrom = pd.ActiveFrom,
                    ExpirationDate = pd.ExpirationDate,
                    CreatedDate = pd.CreatedDate,
                    UpdatedDate = pd.UpdatedDate
                }).ToList(),
                Images = property.Images?.Select(pi => new PropertyImage
                {
                    Id = pi.Id,
                    Description = pi.Description,
                    FileName = pi.FileName,
                    FilePath = pi.FilePath,
                    FileType = pi.FileType,
                    CreatedDate = pi.CreatedDate,
                    UpdatedDate = pi.UpdatedDate
                }).ToList(),
                Portfolio = new PortfolioDetailsDto
                {
                    Id = property.Portfolio.Id,
                    Name = property.Portfolio.Name
                },
                Tenants = property.Tenants?.Select(t => new TenantDTO
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    IsSmoker = t.IsSmoker,
                    HasPets = t.HasPets,
                    EmailAddress = t.EmailAddress,
                    DateOfBirth = t.DateOfBirth,
                    JobTitle = t.JobTitle,
                }).ToList(),
                UpdatedDate = property.UpdatedDate
            }).AsEnumerable();

            return View("SearchResults", resultsDto);
        }
    }
}