using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using Website.Data;
using Website.Extensions.Alerts;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.PropertyDocuments;

namespace Website.Controllers
{
    public class PropertyDocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPropertyDocumentService _propertyDocumentService;

        public PropertyDocumentsController(ApplicationDbContext context, IMapper mapper, IPropertyDocumentService propertyDocumentService)
        {
            _context = context;
            _mapper = mapper;
            _propertyDocumentService = propertyDocumentService;
        }

        // GET: PropertyDocuments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PropertyDocuments.Include(p => p.DocumentType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PropertyDocuments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyDocument = await _context.PropertyDocuments
                .Include(p => p.DocumentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyDocument == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<PropertyDocumentDetailsDto>(propertyDocument);

            return View(result);
        }

        // GET: PropertyDocuments/Create
        public IActionResult Create(Guid propertyId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes.Where(x => x.Owner == null || x.OwnerId == userId), "Id", "Description");
            var data = new PropertyDocumentCreateDto
            {
                PropertyId = propertyId,
            };
            return View(data);
        }

        // POST: PropertyDocuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Document,DocumentTypeId,PropertyId")] PropertyDocumentCreateDto propertyDocument)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                try
                {
                    var property = await _context.Properties.Include(x => x.Portfolio).SingleOrDefaultAsync(x => x.Id == propertyDocument.PropertyId);
                    var result = await _propertyDocumentService.CreatePropertyDocumentForProperty(propertyDocument.PropertyId, propertyDocument.Document, propertyDocument.DocumentTypeId);
                    return RedirectToAction("GetPropertyById", "Property", new { portfolioId = property.Portfolio.Id, propertyId = property.Id }).WithSuccess("Success", "Document Added");
                }
                catch (ImageFormatLimitationException ex)
                {
                    ModelState.AddModelError("Document", ex.Message);
                }
            }
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes.Where(x => x.Owner == null || x.OwnerId == userId), "Id", "Description");
            return View(propertyDocument);
        }

        // GET: PropertyDocuments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyDocument = await _context.PropertyDocuments.FindAsync(id);
            if (propertyDocument == null)
            {
                return NotFound();
            }
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Id", propertyDocument.DocumentTypeId);
            return View(propertyDocument);
        }

        // POST: PropertyDocuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FileName,FilePath,FileType,DocumentTypeId,ExpirationDate,Id,CreatedDate,UpdatedDate")] PropertyDocument propertyDocument)
        {
            if (id != propertyDocument.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(propertyDocument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyDocumentExists(propertyDocument.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DocumentTypeId"] = new SelectList(_context.DocumentTypes, "Id", "Id", propertyDocument.DocumentTypeId);
            return View(propertyDocument);
        }

        // GET: PropertyDocuments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyDocument = await _context.PropertyDocuments
                .Include(p => p.DocumentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyDocument == null)
            {
                return NotFound();
            }

            return View(propertyDocument);
        }

        // POST: PropertyDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var propertyDocument = await _context.PropertyDocuments.FindAsync(id);
            _context.PropertyDocuments.Remove(propertyDocument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyDocumentExists(Guid id)
        {
            return _context.PropertyDocuments.Any(e => e.Id == id);
        }
    }
}