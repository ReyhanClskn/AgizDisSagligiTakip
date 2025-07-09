using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.ViewModels
{
    public class KullaniciKayitViewModel
    {
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir.")]
        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir.")]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(150, ErrorMessage = "E-posta en fazla 150 karakter olabilir.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        //TODO: -düzeldi
        // For strong passwords
        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "Şifre tekrar alanı zorunludur.")]
        [Compare("Sifre", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string SifreTekrar { get; set; }

        [Required(ErrorMessage = "Doğum tarihi alanı zorunludur.")]
        [Display(Name = "Doğum Tarihi")]
        public DateTime DogumTarihi { get; set; }
    }
}