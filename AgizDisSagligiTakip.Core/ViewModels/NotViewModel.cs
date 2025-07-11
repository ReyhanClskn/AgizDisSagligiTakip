using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligiTakip.Core.ViewModels
{
    public class NotViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        [Display(Name = "Görsel Yolu")]
        public string GorselYolu { get; set; } = "";

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; }

        public int KullaniciId { get; set; }
    }
}