using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AgizDisSagligiTakip.Core.ViewModels
{
    public class ProfilViewModel
    {
        public int Id { get; set; }

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

        [Required(ErrorMessage = "Doğum tarihi alanı zorunludur.")]
        [Display(Name = "Doğum Tarihi")]
        public DateTime DogumTarihi { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime KayitTarihi { get; set; }

        [Display(Name = "Yeni Şifre")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
        public string? YeniSifre { get; set; }

        [Display(Name = "Yeni Şifre Tekrar")]
        [Compare("YeniSifre", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string? YeniSifreTekrar { get; set; }

        // Hesap silme için şifre doğrulama
        [Display(Name = "Mevcut Şifre")]
        [DataType(DataType.Password)]
        public string? MevcutSifre { get; set; }

        // Profil fotoğrafı
        [Display(Name = "Profil Fotoğrafı")]
        public string? ProfilFotoUrl { get; set; }

        // Yeni profil fotoğrafı yükleme
        [Display(Name = "Yeni Profil Fotoğrafı")]
        public IFormFile? ProfilFoto { get; set; }
    }
}