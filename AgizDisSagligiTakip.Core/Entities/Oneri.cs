using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.Entities
{
    public class Oneri
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Metin { get; set; }
        
        [Required]
        public bool Aktif { get; set; } = true;
        
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    }
}