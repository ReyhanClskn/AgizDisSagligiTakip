using Microsoft.AspNetCore.Mvc;
using AgizDisSagligiTakip.Core.ViewModels;
using AgizDisSagligiTakip.Core.Entities;
using AgizDisSagligiTakip.Data.Context;
using AgizDisSagligiTakip.Core.Helpers;
using System.Text;
using System.Security.Cryptography;

namespace AgizDisSagligiTakip.Web.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly EmailService _emailService;
        private readonly SifreleyiciService _sifreleyici;

        public KullaniciController(UygulamaDbContext context)
        {
            _context = context;
            _emailService = new EmailService();
            _sifreleyici = new SifreleyiciService();
        }

        // GET: Kullanici/Kayit
        public IActionResult Kayit()
        {
            return View();
        }

        // POST: Kullanici/Kayit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kayit(KullaniciKayitViewModel model)
        {
            if (ModelState.IsValid)
            {
                // E-posta kontrolü
                var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email);
                if (mevcutKullanici != null)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılmaktadır.");
                    return View(model);
                }

                var sifrelenmisSifre = SifreyiSifrele(model.Sifre);

                // Yeni kullanıcı oluştur
                var yeniKullanici = new Kullanici
                {
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    Email = model.Email,
                    Sifre = sifrelenmisSifre,  //TODO:
                    DogumTarihi = model.DogumTarihi,
                    KayitTarihi = DateTime.Now
                };

                _context.Kullanicilar.Add(yeniKullanici);
                _context.SaveChanges();

                // Email gönderimi
                try
                {
                    var emailGonderildi = await _emailService.KayitMailiGonderAsync(model.Email, model.Ad);
                    if (emailGonderildi)
                    {
                        TempData["BasariMesaji"] = "Kayıt işlemi başarıyla tamamlandı! Onay maili gönderildi.";
                    }
                    else
                    {
                        TempData["BasariMesaji"] = "Kayıt işlemi başarıyla tamamlandı! (Mail gönderiminde sorun oluştu)";
                    }
                }
                catch
                {
                    TempData["BasariMesaji"] = "Kayıt işlemi başarıyla tamamlandı! (Mail servisi şu anda çalışmıyor)";
                }

                return RedirectToAction("Giris");
            }

            return View(model);
        }

        // GET: Kullanici/Giris
        public IActionResult Giris()
        {
            return View();
        }

        // POST: Kullanici/Giris
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Giris(KullaniciGirisViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email);
                
                if (kullanici == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return View(model);
                }

                var sifreliGirilen = _sifreleyici.Sifrele(model.Sifre);
                var kontrolSonucu = SifreKontrolEt(model.Sifre, kullanici.Sifre);

                // Şifre kontrolü
                if (!SifreKontrolEt(model.Sifre, kullanici.Sifre))
                {
                    ModelState.AddModelError("", "Şifre hatalı.");
                    return View(model);
                }

                // Session'a kullanıcı bilgisini kaydet
                HttpContext.Session.SetInt32("KullaniciId", kullanici.Id);
                HttpContext.Session.SetString("KullaniciAd", kullanici.Ad);
                HttpContext.Session.SetString("KullaniciSoyad", kullanici.Soyad);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: Kullanici/Cikis
        public IActionResult Cikis()
        {
            // Session'ı temizle
            HttpContext.Session.Clear();
            
            // Ana sayfaya yönlendir
            TempData["BilgiMesaji"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Index", "Home");
        }

        // GET: Kullanici/Profil
        public IActionResult Profil()
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("Giris");
            }

            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Id == kullaniciId);
            if (kullanici == null)
            {
                return RedirectToAction("Giris");
            }

            var model = new ProfilViewModel
            {
                Id = kullanici.Id,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                Email = kullanici.Email,
                DogumTarihi = kullanici.DogumTarihi,
                KayitTarihi = kullanici.KayitTarihi
            };

            return View(model);
        }

        // POST: Kullanici/Profil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profil(ProfilViewModel model)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("Giris");
            }

            if (ModelState.IsValid)
            {
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Id == kullaniciId);
                if (kullanici == null)
                {
                    return RedirectToAction("Giris");
                }

                // Email değişikliği kontrolü
                if (kullanici.Email != model.Email)
                {
                    var mevcutEmail = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email && k.Id != kullaniciId);
                    if (mevcutEmail != null)
                    {
                        ModelState.AddModelError("Email", "Bu e-posta adresi başka bir kullanıcı tarafından kullanılmaktadır.");
                        return View(model);
                    }
                }

                // Bilgileri güncelle
                kullanici.Ad = model.Ad;
                kullanici.Soyad = model.Soyad;
                kullanici.Email = model.Email;
                kullanici.DogumTarihi = model.DogumTarihi;

                // Şifre güncellemesi
                if (!string.IsNullOrEmpty(model.YeniSifre))
                {
                    kullanici.Sifre = SifreyiSifrele(model.YeniSifre);
                }

                _context.SaveChanges();

                // Session'daki bilgileri güncelle
                HttpContext.Session.SetString("KullaniciAd", kullanici.Ad);
                HttpContext.Session.SetString("KullaniciSoyad", kullanici.Soyad);

                TempData["BasariMesaji"] = "Profil bilgileriniz başarıyla güncellendi!";
                return RedirectToAction("Profil");
            }

            return View(model);
        }

        // GET: Kullanici/ParolaHatirlat
        public IActionResult ParolaHatirlat()
        {
            return View();
        }

        // POST: Kullanici/ParolaHatirlat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ParolaHatirlat(ParolaHatirlatViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // İlk adım: Email kontrolü
            if (!model.EmailDogrulandi)
            {
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email);
                if (kullanici == null)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresine kayıtlı kullanıcı bulunamadı.");
                    return View(model);
                }

                // Email doğru, şifre alanlarını göster
                model.EmailDogrulandi = true;
                ModelState.Clear();
                return View(model);
            }

            // İkinci adım: Şifre güncelleme TODO:
            if (string.IsNullOrEmpty(model.YeniSifre) || string.IsNullOrEmpty(model.YeniSifreTekrar))
            {
                ModelState.AddModelError("", "Lütfen yeni şifrenizi giriniz.");
                return View(model);
            }

            var guncellenecekKullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email);
            if (guncellenecekKullanici != null)
            {
                guncellenecekKullanici.Sifre = SifreyiSifrele(model.YeniSifre);
                _context.SaveChanges();

                TempData["BasariMesaji"] = "Şifreniz başarıyla güncellendi!";
                return RedirectToAction("Giris");
            }

            ModelState.AddModelError("", "Bir hata oluştu. Lütfen tekrar deneyin.");
            return View(model);
        }

        // AES Şifreleme TODO: Kriptoloji notlarına bak
        private string SifreyiSifrele(string sifre)
        {
            return _sifreleyici.Sifrele(sifre);
        }

        private bool SifreKontrolEt(string girilenSifre, string veritabanindakiSifre)
        {
            return _sifreleyici.SifreKontrolEt(girilenSifre, veritabanindakiSifre);
        }
    }
}