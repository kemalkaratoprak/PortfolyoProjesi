using System.ComponentModel.DataAnnotations;

namespace PortfolyoProjesi.Models
{
    public class VisitorCounter
    {
        [Key]
        public int Id { get; set; }

        public int TotalVisits { get; set; } // Toplam ziyaret sayısını burada tutacağız
    }
}