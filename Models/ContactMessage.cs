using System.ComponentModel.DataAnnotations;

namespace PortfolyoProjesi.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Subject { get; set; }
        
        [Required]
        public string Message { get; set; }

        // --- YENİ EKLENENLER ---
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Mesajın geldiği tarih
        public bool IsRead { get; set; } = false; // Okundu bilgisi (Varsayılan: Okunmadı)
    }
}