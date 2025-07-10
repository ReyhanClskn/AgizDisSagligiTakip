using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.ViewModels
{
    public class ParolaHatirlatKodViewModel
    {
        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Doğrulama Kodu")]
        public string? DogrulamaKodu { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
        [Display(Name = "Yeni Şifre")]
        public string? YeniSifre { get; set; }

        [Compare("YeniSifre", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Yeni Şifre Tekrar")]
        public string? YeniSifreTekrar { get; set; }

        public string Adim { get; set; } = "EmailGirisi"; // EmailGirisi, KodDogrulama, SifreGuncelleme TODO:
        public string? GonderilmisKod { get; set; }
        public DateTime? KodGondermeZamani { get; set; }
    }
}