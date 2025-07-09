using System.ComponentModel.DataAnnotations;
using AgizDisSagligiTakip.Core.Enums;

namespace AgizDisSagligiTakip.Core.Entities
{
    public class Hedef
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Baslik { get; set; }
        
        [StringLength(500)]
        public string Aciklama { get; set; }
        
        [Required]
        public PeriyotTuru Periyot { get; set; }
        
        [Required]
        public OnemDerecesi OnemDerecesi { get; set; }
        
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        
        // Foreign Key
        public int KullaniciId { get; set; }
        
        // Navigation Properties
        public virtual Kullanici Kullanici { get; set; }
        public virtual ICollection<HedefKaydi> HedefKayitlari { get; set; } = new List<HedefKaydi>();
    }
}