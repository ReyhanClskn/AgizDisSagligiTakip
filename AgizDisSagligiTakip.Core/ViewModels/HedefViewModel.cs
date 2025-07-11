using System.ComponentModel.DataAnnotations;
using AgizDisSagligiTakip.Core.Enums;

namespace AgizDisSagligiTakip.Core.ViewModels
{
    public class HedefViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        [Display(Name = "Başlık")]
        public string Baslik { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; } = "";

        [Required(ErrorMessage = "Periyot seçimi zorunludur.")]
        [Display(Name = "Periyot")]
        public PeriyotTuru Periyot { get; set; }

        [Required(ErrorMessage = "Önem derecesi seçimi zorunludur.")]
        [Display(Name = "Önem Derecesi")]
        public OnemDerecesi OnemDerecesi { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; }

        // Silme kontrolü için hedef kayıtları sayısı
        public int KayitSayisi { get; set; }
    }
}