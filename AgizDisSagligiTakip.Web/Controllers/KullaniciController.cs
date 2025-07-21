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

                // 6 haneli doğrulama kodu oluştur
                var dogrulamaKodu = new Random().Next(100000, 999999).ToString();
                
                // Kullanıcı bilgilerini geçici olarak TempData'da sakla
                TempData["TempKullaniciAd"] = model.Ad;
                TempData["TempKullaniciSoyad"] = model.Soyad;
                TempData["TempKullaniciEmail"] = model.Email;
                TempData["TempKullaniciSifre"] = SifreyiSifrele(model.Sifre);
                TempData["TempKullaniciDogumTarihi"] = model.DogumTarihi.ToString();
                TempData["TempDogrulamaKodu"] = dogrulamaKodu;
                TempData["TempDogrulamaKoduSuresi"] = DateTime.Now.AddMinutes(10).ToString();

                // Email gönderimi
                try
                {
                    var emailGonderildi = await _emailService.DogrulamaMailiGonderAsync(
                        model.Email, 
                        model.Ad, 
                        dogrulamaKodu
                    );

                    if (emailGonderildi)
                    {
                        TempData["BasariMesaji"] = "Doğrulama kodu email adresinize gönderildi!";
                        return RedirectToAction("EmailDogrula", new { email = model.Email });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email gönderiminde bir hata oluştu. Lütfen tekrar deneyin.");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Email servisi şu anda çalışmıyor. Lütfen daha sonra tekrar deneyin.");
                    return View(model);
                }
            }

            return View(model);
        }

        // GET: Kullanici/EmailDogrula
        public IActionResult EmailDogrula(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Kayit");
            }

            var model = new EmailDogrulamaViewModel
            {
                Email = email
            };

            return View(model);
        }

        // POST: Kullanici/EmailDogrula
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailDogrula(EmailDogrulamaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TempData'dan bilgileri al
                var tempEmail = TempData["TempKullaniciEmail"]?.ToString();
                var tempDogrulamaKodu = TempData["TempDogrulamaKodu"]?.ToString();
                var tempDogrulamaKoduSuresi = TempData["TempDogrulamaKoduSuresi"]?.ToString();

                // Email kontrolü
                if (tempEmail != model.Email)
                {
                    ModelState.AddModelError("", "Email adresi eşleşmiyor.");
                    return View(model);
                }

                // Süre kontrolü
                if (!string.IsNullOrEmpty(tempDogrulamaKoduSuresi))
                {
                    var sureBitis = DateTime.Parse(tempDogrulamaKoduSuresi);
                    if (DateTime.Now > sureBitis)
                    {
                        ModelState.AddModelError("DogrulamaKodu", "Doğrulama kodu süresi dolmuş. Lütfen tekrar kayıt olun.");
                        return View(model);
                    }
                }

                // Doğrulama kodu kontrolü
                if (tempDogrulamaKodu != model.DogrulamaKodu)
                {
                    ModelState.AddModelError("DogrulamaKodu", "Doğrulama kodu hatalı.");
                    // TempData'yı geri yükle
                    TempData.Keep();
                    return View(model);
                }

                // Doğrulama başarılı, kullanıcıyı kaydet
                var yeniKullanici = new Kullanici
                {
                    Ad = TempData["TempKullaniciAd"]?.ToString(),
                    Soyad = TempData["TempKullaniciSoyad"]?.ToString(),
                    Email = tempEmail,
                    Sifre = TempData["TempKullaniciSifre"]?.ToString(),
                    DogumTarihi = DateTime.Parse(TempData["TempKullaniciDogumTarihi"]?.ToString()),
                    KayitTarihi = DateTime.Now,
                    EmailDogrulandi = true
                };

                _context.Kullanicilar.Add(yeniKullanici);
                _context.SaveChanges();

                // Kayıt tamamlandı emaili gönder
                try
                {
                    await _emailService.KayitMailiGonderAsync(yeniKullanici.Email, yeniKullanici.Ad);
                }
                catch
                {
                    // Email gönderilemese bile kayıt tamamlandı
                }

                TempData["BasariMesaji"] = "Kayıt işlemi başarıyla tamamlandı! Giriş yapabilirsiniz.";
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

                // Email doğrulanmamışsa giriş yapmasın
                if (!kullanici.EmailDogrulandi)
                {
                    ModelState.AddModelError("", "Email adresinizi doğrulamanız gerekmektedir.");
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
            var model = new ParolaHatirlatViewModel
            {
                EmailDogrulandi = false,
                DogrulamaKoduGonderildi = false
            };
            
            return View(model);
        }

        // POST: Kullanici/ParolaHatirlat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ParolaHatirlat(ParolaHatirlatViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Adım: Email kontrolü ve doğrulama kodu gönderme
            if (!model.DogrulamaKoduGonderildi)
            {
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email);
                if (kullanici == null)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresine kayıtlı kullanıcı bulunamadı.");
                    return View(model);
                }

                // 6 haneli doğrulama kodu oluştur
                var dogrulamaKodu = new Random().Next(100000, 999999).ToString();
                
                // Kullanıcıya doğrulama kodu kaydet (10 dakika geçerli)
                kullanici.DogrulamaKodu = dogrulamaKodu;
                kullanici.DogrulamaKoduSuresi = DateTime.Now.AddMinutes(10);
                _context.SaveChanges();

                // Email gönder
                try
                {
                    var emailGonderildi = await _emailService.DogrulamaMailiGonderAsync(
                        model.Email, 
                        kullanici.Ad, 
                        dogrulamaKodu
                    );

                    if (emailGonderildi)
                    {
                        model.DogrulamaKoduGonderildi = true;
                        TempData["BasariMesaji"] = "Doğrulama kodu email adresinize gönderildi!";
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email gönderiminde bir hata oluştu. Lütfen tekrar deneyin.");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Email servisi şu anda çalışmıyor. Lütfen daha sonra tekrar deneyin.");
                    return View(model);
                }
            }

            // 2. Adım: Doğrulama kodu kontrolü ve şifre güncelleme
            if (model.DogrulamaKoduGonderildi && !model.EmailDogrulandi)
            {
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email);
                if (kullanici == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return View(model);
                }

                // Doğrulama kodu kontrolü
                if (string.IsNullOrEmpty(kullanici.DogrulamaKodu) || 
                    kullanici.DogrulamaKodu != model.DogrulamaKodu)
                {
                    ModelState.AddModelError("DogrulamaKodu", "Doğrulama kodu hatalı.");
                    model.DogrulamaKoduGonderildi = true;
                    return View(model);
                }

                // Süre kontrolü
                if (kullanici.DogrulamaKoduSuresi == null || 
                    DateTime.Now > kullanici.DogrulamaKoduSuresi)
                {
                    ModelState.AddModelError("DogrulamaKodu", "Doğrulama kodu süresi dolmuş. Yeni kod isteyin.");
                    model.DogrulamaKoduGonderildi = false; // Yeni kod isteyebilsin
                    return View(model);
                }

                // Şifre doğrulama
                if (string.IsNullOrEmpty(model.YeniSifre) || string.IsNullOrEmpty(model.YeniSifreTekrar))
                {
                    ModelState.AddModelError("", "Lütfen yeni şifrenizi giriniz.");
                    model.DogrulamaKoduGonderildi = true;
                    return View(model);
                }

                if (model.YeniSifre != model.YeniSifreTekrar)
                {
                    ModelState.AddModelError("YeniSifreTekrar", "Şifreler eşleşmiyor.");
                    model.DogrulamaKoduGonderildi = true;
                    return View(model);
                }

                // Şifreyi güncelle
                kullanici.Sifre = SifreyiSifrele(model.YeniSifre);
                
                // Doğrulama kodunu temizle
                kullanici.DogrulamaKodu = null;
                kullanici.DogrulamaKoduSuresi = null;
                
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