using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Website.Data;
using Website.Interfaces;
using Website.Models;
using Website.Models.DTOs.PropertyImage;

namespace Website.Controllers
{
    public class PropertyImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPropertyImageService _propertyImageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertyImagesController(ApplicationDbContext context, IMapper mapper, IPropertyImageService propertyImageService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _propertyImageService = propertyImageService;
            _userManager = userManager;
        }

        // GET: PropertyImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.PropertyImages.ToListAsync());
        }

        // GET: PropertyImages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyImage = await _context.PropertyImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyImage == null)
            {
                return NotFound();
            }

            return View(propertyImage);
        }

        // GET: PropertyImages/Create
        public IActionResult Create(Guid propertyId)
        {
            return View();
        }

        // POST: PropertyImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Image,PropertyId")] PropertyImageCreateDto propertyImage)
        {
            if (ModelState.IsValid)
            {
                var property = await _context.Properties.Include(x => x.Portfolio).SingleOrDefaultAsync(x => x.Id == propertyImage.PropertyId);
                var file = await _propertyImageService.CreateImageForProperty(property, propertyImage.Image);
                return RedirectToAction("GetPropertyById", nameof(PropertyController), new { portfolioId = property.Portfolio.Id, propertyId = propertyImage.PropertyId });
            }
            return View(propertyImage);
        }

        // GET: PropertyImages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyImage = await _context.PropertyImages.FindAsync(id);
            if (propertyImage == null)
            {
                return NotFound();
            }
            return View(propertyImage);
        }

        // POST: PropertyImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FileName,FilePath,FileType,Id,CreatedDate,UpdatedDate")] PropertyImage propertyImage)
        {
            if (id != propertyImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(propertyImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyImageExists(propertyImage.Id))
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
            return View(propertyImage);
        }

        // GET: PropertyImages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyImage = await _context.PropertyImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyImage == null)
            {
                return NotFound();
            }

            return View(propertyImage);
        }

        // POST: PropertyImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var propertyImage = await _context.PropertyImages.FindAsync(id);
            _context.PropertyImages.Remove(propertyImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyImageExists(Guid id)
        {
            return _context.PropertyImages.Any(e => e.Id == id);
        }
    }
}