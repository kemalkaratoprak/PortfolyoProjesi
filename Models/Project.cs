using System.ComponentModel.DataAnnotations;

namespace PortfolyoProjesi.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        // --- YENİ EKLENEN ALANLAR ---
        
        public string? GithubUrl { get; set; } // GitHub kaynak kod linki

        public string? LiveUrl { get; set; }   // Varsa canlı site linki
    }
}