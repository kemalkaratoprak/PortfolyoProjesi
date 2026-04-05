using Microsoft.EntityFrameworkCore;

namespace PortfolyoProjesi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Veri tabanında 'Projects' adında bir tablo oluşturulmasını sağlar
        public DbSet<Project> Projects { get; set; } 
    }
}