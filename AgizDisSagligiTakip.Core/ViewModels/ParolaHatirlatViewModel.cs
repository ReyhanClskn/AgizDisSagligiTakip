using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.ViewModels
{
    public class ParolaHatirlatViewModel
    {
        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
        [Display(Name = "Yeni Şifre")]
        public string? YeniSifre { get; set; }

        [Compare("YeniSifre", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Yeni Şifre Tekrar")]
        public string? YeniSifreTekrar { get; set; }

        public bool EmailDogrulandi { get; set; } = false;

        // Email doğrulama için     
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Doğrulama kodu 6 haneli olmalıdır.")]
        [Display(Name = "Doğrulama Kodu")]
        public string? DogrulamaKodu { get; set; }

        public bool DogrulamaKoduGonderildi { get; set; } = false;
    }
}