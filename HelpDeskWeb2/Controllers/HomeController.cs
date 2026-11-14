using HelpDeskWeb2.Data;
using HelpDeskWeb2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelpDeskWeb2.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalTickets = _context.Tickets.Count();
            return View();
        }
        public IActionResult Privacy()
        {
            ViewBag.TotalTickets = _context.Tickets.Count();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}