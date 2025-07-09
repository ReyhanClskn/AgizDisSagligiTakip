using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.Entities
{
    public class Not
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Aciklama { get; set; }
        
        [StringLength(255)]
        public string GorselYolu { get; set; }
        
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        
        // Foreign Key
        public int KullaniciId { get; set; }
        
        // Navigation Property
        public virtual Kullanici Kullanici { get; set; }
    }
}