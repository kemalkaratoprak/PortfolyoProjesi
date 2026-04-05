using System.ComponentModel.DataAnnotations;

namespace PortfolyoProjesi.Models
{
    public class Project
    {
        [Key] // Bu alanın birincil anahtar (Primary Key) olduğunu belirtir
        public int Id { get; set; }

        public string Title { get; set; } // Projenin Başlığı

        public string Description { get; set; } // Projenin Açıklaması

        public string ImageUrl { get; set; } // Projenin Görsel Linki
    }
}