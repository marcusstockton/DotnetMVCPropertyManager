using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Website.Data;
using Website.Models;
using Website.Models.DTOs.DocumentTypes;

namespace Website.Controllers
{
    [Authorize]
    public class DocumentTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DocumentTypesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocumentTypes
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var docTypes = _context.DocumentTypes.Where(x => x.Owner == user || x.Owner == null);

            var results = docTypes.Select(dt => new DocumentTypeDetailDto
            {
                Id = dt.Id,
                Description = dt.Description,
                Expires = dt.Expires,
                ExpiryDate = dt.ExpiryDate,
                OwnerId = dt.OwnerId,
                Owner = dt.Owner,
                CreatedDate = dt.CreatedDate,
                UpdatedDate = dt.UpdatedDate
            }).ToList();

            return View(results);
        }

        // GET: DocumentTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = await _context.DocumentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documentType == null)
            {
                return NotFound();
            }

            var dto = new DocumentTypeDetailDto
            {
                Id = documentType.Id,
                Description = documentType.Description,
                Expires = documentType.Expires,
                ExpiryDate = documentType.ExpiryDate,
                OwnerId = documentType.OwnerId,
                Owner = documentType.Owner,
                CreatedDate = documentType.CreatedDate,
                UpdatedDate = documentType.UpdatedDate
            };

            return View(dto);
        }

        // GET: DocumentTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DocumentTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Expires,ExpiryDate")] DocumentTypeCreateDto documentType)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Owner"))
                {
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    documentType.Owner = user;
                }

                documentType.Id = Guid.NewGuid();

                var mappedData = new DocumentType
                {
                    Id = documentType.Id,
                    Description = documentType.Description,
                    Expires = documentType.Expires,
                    ExpiryDate = documentType.ExpiryDate,
                    OwnerId = documentType.OwnerId,
                    Owner = documentType.Owner
                };

                _context.Add(mappedData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(documentType);
        }

        // GET: DocumentTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = await _context.DocumentTypes.FindAsync(id);
            if (documentType == null)
            {
                return NotFound();
            }
            return View(documentType);
        }

        // POST: DocumentTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,Id,CreatedDate,UpdatedDate,Expires,ExpiryDate,Owner,OwnerId")] DocumentType documentType)
        {
            if (id != documentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentTypeExists(documentType.Id))
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
            return View(documentType);
        }

        // GET: DocumentTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = await _context.DocumentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }

        // POST: DocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var documentType = await _context.DocumentTypes.FindAsync(id);
            _context.DocumentTypes.Remove(documentType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentTypeExists(Guid id)
        {
            return _context.DocumentTypes.Any(e => e.Id == id);
        }
    }
}