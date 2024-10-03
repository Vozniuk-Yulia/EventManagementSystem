using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
           
        }
        [HttpGet]
        public IActionResult Index()
        {
            var locations = _context.Locations.ToList();
            return View(locations);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create( Location location, IFormFile Photo)
        {
            
                if (Photo != null && Photo.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var filePath = Path.Combine(uploads, Photo.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Photo.CopyToAsync(fileStream);
                    }
                    location.Photo = "/uploads/" + Photo.FileName;
                }
                
                    _context.Add(location);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                



        }
        public async Task<IActionResult> Delete(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Location location, IFormFile Photo)
        {

            var locationFromDb = await _context.Locations.FindAsync(location.Id);
            if (locationFromDb == null)
            {
                return NotFound();
            }

            locationFromDb.Name = location.Name;
            locationFromDb.Address = location.Address;
            locationFromDb.Description = location.Description;

            if (Photo != null && Photo.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var filePath = Path.Combine(uploads, Photo.FileName);

   
                if (!string.IsNullOrEmpty(locationFromDb.Photo))
                {
                    var oldFilePath = Path.Combine(uploads, Path.GetFileName(locationFromDb.Photo));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(fileStream);
                }
                locationFromDb.Photo = "/uploads/" + Photo.FileName;
            }

            _context.Update(locationFromDb);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
