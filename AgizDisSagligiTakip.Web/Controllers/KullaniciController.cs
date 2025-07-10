using Microsoft.AspNetCore.Mvc;
using AgizDisSagligiTakip.Core.ViewModels;
using AgizDisSagligiTakip.Core.Entities;
using AgizDisSagligiTakip.Data.Context;
using System.Text;
using System.Security.Cryptography;

namespace AgizDisSagligiTakip.Web.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly UygulamaDbContext _context;

        public KullaniciController(UygulamaDbContext context)
        {
            _context = context;
        }

        // GET: Kullanici/Kayit
        public IActionResult Kayit()
        {
            return View();
        }

        // POST: Kullanici/Kayit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Kayit(KullaniciKayitViewModel model)
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

                // Yeni kullanıcı oluştur
                var yeniKullanici = new Kullanici
                {
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    Email = model.Email,
                    Sifre = SifreyiSifrele(model.Sifre),
                    DogumTarihi = model.DogumTarihi,
                    KayitTarihi = DateTime.Now
                };

                _context.Kullanicilar.Add(yeniKullanici);
                _context.SaveChanges();

                // Başarı mesajı
                TempData["BasariMesaji"] = "Kayıt işlemi başarıyla tamamlandı!";
                return RedirectToAction("Giris");
            }

            return View(model);
        }

        // GET: Kullanici/Giris
        public IActionResult Giris()
        {
            //Console.WriteLine("=== GIRIS GET METODU ÇALIŞTI ===");
            return View();
        }

        // POST: Kullanici/Giris
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Giris(KullaniciGirisViewModel model)
        {
            // DEBUG 
            //Console.WriteLine("=== GIRIS POST METODU ÇALIŞTI ===");
            //Console.WriteLine($"Email: {model?.Email}");
            //Console.WriteLine($"Sifre: {model?.Sifre}");
            //Console.WriteLine($"ModelState Valid: {ModelState.IsValid}");
            
            if (ModelState.IsValid)
            {
                //Console.WriteLine("ModelState geçerli, kullanıcı aranıyor...");
                
                // Kullanıcıyı bul
                // Kullanıcıyı bul
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email);
                
                if (kullanici == null)
                {
                    //Console.WriteLine("Kullanıcı bulunamadı!");
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return View(model);
                }

                //Console.WriteLine("Kullanıcı bulundu, şifre kontrol ediliyor...");
                
                // Şifre kontrolü
                // Şifre kontrolü
                var sifrelenmisSifre = SifreyiSifrele(model.Sifre!);
                if (kullanici.Sifre != sifrelenmisSifre)
                {
                    //Console.WriteLine("Şifre hatalı!");
                    ModelState.AddModelError("", "Şifre hatalı.");
                    return View(model);
                }

                //Console.WriteLine("Şifre doğru, session oluşturuluyor...");
                
                // Session'a kullanıcı bilgisini kaydet
                HttpContext.Session.SetInt32("KullaniciId", kullanici.Id);
                HttpContext.Session.SetString("KullaniciAd", kullanici.Ad);
                HttpContext.Session.SetString("KullaniciSoyad", kullanici.Soyad);

                //Console.WriteLine("Başarılı giriş, ana sayfaya yönlendiriliyor...");
                return RedirectToAction("Index", "Home");
            }

            //Console.WriteLine("ModelState geçersiz!");
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

        // GET: Kullanici/ParolaHatirlat TODO:
        public IActionResult ParolaHatirlat()
        {
            var model = new ParolaHatirlatKodViewModel();
            return View(model);
        }

        // POST: Kullanici/ParolaHatirlat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ParolaHatirlat(ParolaHatirlatKodViewModel model)
        {
            if (model.Adim == "EmailGirisi")
            {
                // 1. Adım: Email kontrolü
                if (string.IsNullOrEmpty(model.Email))
                {
                    ModelState.AddModelError("Email", "E-posta adresi gereklidir.");
                    return View(model);
                }

                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == model.Email);
                if (kullanici == null)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresine kayıtlı kullanıcı bulunamadı.");
                    return View(model);
                }

                // Rastgele 6 haneli kod oluştur
                var random = new Random();
                var kod = random.Next(100000, 999999).ToString();

                // Session'a kaydet
                HttpContext.Session.SetString("ParolaHatirlatKod", kod);
                HttpContext.Session.SetString("ParolaHatirlatEmail", model.Email);
                HttpContext.Session.SetString("KodGondermeZamani", DateTime.Now.ToString());

                // Konsola yazdır (gerçek uygulamada email gönderilir)
                Console.WriteLine($"=== PAROLA HATIRLATMA KODU ===");
                Console.WriteLine($"Email: {model.Email}");
                Console.WriteLine($"Kod: {kod}");
                Console.WriteLine($"===========================");

                model.Adim = "KodDogrulama";
                TempData["BilgiMesaji"] = $"Doğrulama kodu oluşturuldu. (Konsola bakınız: {kod})";
                return View(model);
            }
            else if (model.Adim == "KodDogrulama")
            {
                // 2. Adım: Kod doğrulama
                var sessionKod = HttpContext.Session.GetString("ParolaHatirlatKod");
                var sessionEmail = HttpContext.Session.GetString("ParolaHatirlatEmail");

                if (string.IsNullOrEmpty(sessionKod) || sessionEmail != model.Email)
                {
                    ModelState.AddModelError("", "Oturum süresi doldu. Lütfen tekrar deneyin.");
                    return View(new ParolaHatirlatKodViewModel());
                }

                if (model.DogrulamaKodu != sessionKod)
                {
                    ModelState.AddModelError("DogrulamaKodu", "Doğrulama kodu hatalı.");
                    model.Adim = "KodDogrulama";
                    return View(model);
                }

                model.Adim = "SifreGuncelleme";
                return View(model);
            }
            else if (model.Adim == "SifreGuncelleme")
            {
                // 3. Adım: Şifre güncelleme
                if (string.IsNullOrEmpty(model.YeniSifre) || string.IsNullOrEmpty(model.YeniSifreTekrar))
                {
                    ModelState.AddModelError("", "Lütfen yeni şifrenizi giriniz.");
                    model.Adim = "SifreGuncelleme";
                    return View(model);
                }

                if (model.YeniSifre != model.YeniSifreTekrar)
                {
                    ModelState.AddModelError("YeniSifreTekrar", "Şifreler eşleşmiyor.");
                    model.Adim = "SifreGuncelleme";
                    return View(model);
                }

                var sessionEmail = HttpContext.Session.GetString("ParolaHatirlatEmail");
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.Email == sessionEmail);
                
                if (kullanici != null)
                {
                    kullanici.Sifre = SifreyiSifrele(model.YeniSifre);
                    _context.SaveChanges();

                    // Session'ı temizle
                    HttpContext.Session.Remove("ParolaHatirlatKod");
                    HttpContext.Session.Remove("ParolaHatirlatEmail");
                    HttpContext.Session.Remove("KodGondermeZamani");

                    TempData["BasariMesaji"] = "Şifreniz başarıyla güncellendi!";
                    return RedirectToAction("Giris");
                }
            }

            return View(model);
        }

        // Şifre şifreleme metodu
        // Şifre şifreleme metodu (Basit Hash) TODO: AES olmadı nedense
        private string SifreyiSifrele(string sifre)
        {
            using (var sha256 = SHA256.Create())
            {
                var anahtar = "AgizDisSagligiTakip2024!";
                var kombineSifre = sifre + anahtar;
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(kombineSifre));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}