using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Controllers
{
    public class SpeakersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpeakersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var speakers = _context.Speakers.ToList();
            return View(speakers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Speaker speaker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(speaker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            return View(speaker);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var speaker = await _context.Speakers.FindAsync(id);
            if (speaker != null)
            {
                _context.Speakers.Remove(speaker);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


       
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var speaker = await _context.Speakers.FindAsync(id);
            if (speaker == null)
            {
                return NotFound();
            }
            return View(speaker);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Surname,Name,Middlename,Bio")] Speaker speaker)
        {
            if (id != speaker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speaker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                        throw;
          
                }
                return RedirectToAction(nameof(Index));
            }
            return View(speaker);
        }

    }
}
