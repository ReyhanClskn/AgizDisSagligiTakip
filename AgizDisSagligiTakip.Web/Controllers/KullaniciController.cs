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
        
        // GET: Kullanici/Cikis TODO:
        public IActionResult Cikis()
        {
            // Session'ı temizle
            HttpContext.Session.Clear();

            // Ana sayfaya yönlendir
            TempData["BilgiMesaji"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Index", "Home");
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