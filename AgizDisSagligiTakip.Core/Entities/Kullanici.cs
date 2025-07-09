using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.Entities
{
    public class Kullanici
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Ad { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Soyad { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Sifre { get; set; }
        
        [Required]
        public DateTime DogumTarihi { get; set; }
        
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
        
        // Navigation Properties
        public virtual ICollection<Hedef> Hedefler { get; set; } = new List<Hedef>();
        public virtual ICollection<Not> Notlar { get; set; } = new List<Not>();
    }
}