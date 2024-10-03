using EventManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers
{
    public class SheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SheduleController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
