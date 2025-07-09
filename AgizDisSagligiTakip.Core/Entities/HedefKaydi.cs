using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.Entities
{
    public class HedefKaydi
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime Tarih { get; set; }
        
        [Required]
        public TimeSpan Saat { get; set; }
        
        public int? Sure { get; set; } // dakika cinsinden
        
        [Required]
        public bool Uygulandi { get; set; }
        
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
        
        // Foreign Key
        public int HedefId { get; set; }
        
        // Navigation Property
        public virtual Hedef Hedef { get; set; }
    }
}