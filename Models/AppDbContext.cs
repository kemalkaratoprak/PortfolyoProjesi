using Microsoft.EntityFrameworkCore;

namespace PortfolyoProjesi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Projeler tablosu
        public DbSet<Project> Projects { get; set; } 

        // İletişim mesajları tablosu (BU SATIRI EKLEDİK)
        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}