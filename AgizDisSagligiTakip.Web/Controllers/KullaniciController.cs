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
                await _context.SaveChangesAsync();

                // Başarı mesajı
                TempData["BasariMesaji"] = "Kayıt işlemi başarıyla tamamlandı!";
                return RedirectToAction("Giris");
            }

            return View(model);
        }

        // GET: Kullanici/Giris
        public IActionResult Giris()
        {
            return View();
        }

        // Şifre şifreleme metodu
        private string SifreyiSifrele(string sifre)
        {
            using (var aes = Aes.Create())
            {
                var anahtar = "AgizDisSagligiTakip2024!"; // 24 karakter
                var keyBytes = Encoding.UTF8.GetBytes(anahtar);
                Array.Resize(ref keyBytes, 32); // 32 byte için resize

                aes.Key = keyBytes;
                aes.IV = new byte[16]; // 16 byte IV

                using (var encryptor = aes.CreateEncryptor())
                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(sifre);
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }
}