using Microsoft.EntityFrameworkCore;
using HelpDeskWeb2.Models;

namespace HelpDeskWeb2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
    }
}