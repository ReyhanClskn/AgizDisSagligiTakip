using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.ViewModels
{
    public class EmailDogrulamaViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Doğrulama kodu zorunludur.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Doğrulama kodu 6 haneli olmalıdır.")]
        [Display(Name = "Doğrulama Kodu")]
        public string DogrulamaKodu { get; set; }

        // Kullanıcı bilgilerini geçici olarak saklamak için
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Sifre { get; set; }
        public DateTime DogumTarihi { get; set; }
    }
}