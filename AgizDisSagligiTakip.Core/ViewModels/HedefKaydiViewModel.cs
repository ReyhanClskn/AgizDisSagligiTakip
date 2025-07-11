using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.ViewModels
{
    public class HedefKaydiViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hedef seçimi zorunludur.")]
        public int HedefId { get; set; }

        [Required(ErrorMessage = "Tarih alanı zorunludur.")]
        [Display(Name = "Tarih")]
        public DateTime Tarih { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Saat alanı zorunludur.")]
        [Display(Name = "Saat")]
        public TimeSpan Saat { get; set; } = DateTime.Now.TimeOfDay;

        [Display(Name = "Süre (dakika)")]
        [Range(0, 1440, ErrorMessage = "Süre 0-1440 dakika arasında olmalıdır.")]
        public int? Sure { get; set; }

        [Required(ErrorMessage = "Uygulandı durumu seçilmelidir.")]
        [Display(Name = "Uygulandı")]
        public bool Uygulandi { get; set; } = true;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime KayitTarihi { get; set; }
        public string HedefBaslik { get; set; } = "";
    }
}