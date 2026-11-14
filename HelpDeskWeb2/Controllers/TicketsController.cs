using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDeskWeb2.Data;
using HelpDeskWeb2.Models;

namespace HelpDeskWeb2.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category, string status, string searchString)
        {
            var query = _context.Tickets.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(x => x.Category == category);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(x => x.Title.Contains(searchString) || x.Description.Contains(searchString));

            var tickets = await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
            return View(tickets);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            ticket.CreatedAt = DateTime.Now;
            ticket.Status = "New";
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id) return NotFound();
            if (!ModelState.IsValid) return View(ticket);

            var existing = await _context.Tickets.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Title = ticket.Title;
            existing.Description = ticket.Description;
            existing.Category = ticket.Category;
            existing.Priority = ticket.Priority;
            existing.Status = ticket.Status;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}