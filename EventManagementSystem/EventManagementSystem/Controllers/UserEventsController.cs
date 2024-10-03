using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EventManagementSystem.Controllers
{
    public class UserEventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserEventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //public IActionResult Index()
        //{
        //    ViewData["Formats"] = new SelectList(Enum.GetValues(typeof(Format)).Cast<Format>().Select(f => new { ID = (int)f, Name = f.ToString() }), "ID", "Name");
        //    ViewData["Categories"] = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>().Select(c => new { ID = (int)c, Name = c.ToString() }), "ID", "Name");
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Search(string searchTerm, string format, string category, DateTime? startDate, DateTime? endDate)
        //{
        //    var events = from e in _context.Events.Include(e => e.Location)
        //                 select e;

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        events = events.Where(e => e.Title.Contains(searchTerm));
        //    }

        //    if (!string.IsNullOrEmpty(format) && Enum.TryParse(format, out Format formatEnum))
        //    {
        //        events = events.Where(e => e.Format == formatEnum);
        //    }

        //    if (!string.IsNullOrEmpty(category) && Enum.TryParse(category, out Category categoryEnum))
        //    {
        //        events = events.Where(e => e.Category == categoryEnum);
        //    }

        //    if (startDate.HasValue)
        //    {
        //        events = events.Where(e => e.StartDateTime >= startDate.Value);
        //    }

        //    if (endDate.HasValue)
        //    {
        //        events = events.Where(e => e.EndDateTime <= endDate.Value);
        //    }

        //    ViewData["Formats"] = new SelectList(Enum.GetValues(typeof(Format)).Cast<Format>().Select(f => new { ID = (int)f, Name = f.ToString() }), "ID", "Name");
        //    ViewData["Categories"] = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>().Select(c => new { ID = (int)c, Name = c.ToString() }), "ID", "Name");
        //    ViewData["Speakers"] = new MultiSelectList(await _context.Speakers.ToListAsync(), "Id", "Surname");

        //    return View(await events.ToListAsync());
        //}
        public async Task<IActionResult> Index(string searchTerm, string format, string category, DateTime? startDate, DateTime? endDate)
        {
            var events = from e in _context.Events.Include(e => e.Location)
                         select e;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                events = events.Where(e => e.Title.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(format) && Enum.TryParse(format, out Format formatEnum))
            {
                events = events.Where(e => e.Format == formatEnum);
            }

            if (!string.IsNullOrEmpty(category) && Enum.TryParse(category, out Category categoryEnum))
            {
                events = events.Where(e => e.Category == categoryEnum);
            }

            if (startDate.HasValue)
            {
                events = events.Where(e => e.StartDateTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                events = events.Where(e => e.EndDateTime <= endDate.Value);
            }

            ViewData["Formats"] = new SelectList(Enum.GetValues(typeof(Format)).Cast<Format>().Select(f => new { ID = (int)f, Name = f.ToString() }), "ID", "Name");
            ViewData["Categories"] = new SelectList(Enum.GetValues(typeof(Category)).Cast<Category>().Select(c => new { ID = (int)c, Name = c.ToString() }), "ID", "Name");
            ViewData["Speakers"] = new MultiSelectList(await _context.Speakers.ToListAsync(), "Id", "Surname");

            return View(events);
        }

        [HttpGet]
        public IActionResult Search(string searchTerm, string format, string category, DateTime? startDate, DateTime? endDate)
        {
            return RedirectToAction("Index", new { searchTerm, format, category, startDate, endDate });
        }
        [HttpGet]

        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Location)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View("Details", eventItem);
        }
        [HttpPost]
        [Authorize] 
        public IActionResult RegisterForEvent(int eventId)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var eventToRegister = _context.Events
                                          .Include(e => e.Tickets) 
                                          .FirstOrDefault(e => e.Id == eventId); 

            if (eventToRegister == null)
            {
                TempData["Error"] = "Подія не знайдена.";
                return RedirectToAction("Index", "Home");
            }

           
            var availableTicket = eventToRegister.Tickets.FirstOrDefault(t => t.QuantityAvailable > 0);
            if (availableTicket == null)
            {
                TempData["Error"] = "Немає доступних квитків для цієї події.";
                return RedirectToAction("Details", new { id = eventId });
            }

            var ticket = new Ticket
            {
                EventId = eventId,
                Price = availableTicket.Price,
                QuantityAvailable = availableTicket.QuantityAvailable - 1, 
            };

            _context.Tickets.Update(availableTicket); 
            _context.SaveChanges();

            TempData["Success"] = "Ви успішно зареєстровані на подію!";
            return RedirectToAction("Details", new { id = eventId });
        }
        [Authorize]
        public IActionResult ListEvents()
        {
            var model = new EventSearchViewModel
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1) 
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult ListEvents(EventSearchViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEvents = _context.Events
                .Include(e => e.Users)
                .Include(e => e.Location) 
                .Where(e => e.Users.Any(u => u.Id == userId) && e.StartDateTime >= model.StartDate && e.EndDateTime <= model.EndDate)
                .ToList();

            ViewBag.UserEvents = userEvents;
            return View(model);
        }
        
    }
}
