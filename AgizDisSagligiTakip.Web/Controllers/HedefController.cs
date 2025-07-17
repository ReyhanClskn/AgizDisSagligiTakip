using Microsoft.AspNetCore.Mvc;
using AgizDisSagligiTakip.Data.Context;
using AgizDisSagligiTakip.Core.Entities;
using AgizDisSagligiTakip.Core.Enums;
using AgizDisSagligiTakip.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AgizDisSagligiTakip.Web.Controllers
{
    public class HedefController : Controller
    {
        private readonly UygulamaDbContext _context;

        public HedefController(UygulamaDbContext context)
        {
            _context = context;
        }

        // GET: Hedef/Index (Ana Ağız ve Diş Sağlığı Sayfası)
        public IActionResult Index()
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            // Kullanıcının hedefleri
            var hedefler = _context.Hedefler
                .Where(h => h.KullaniciId == kullaniciId)
                .Include(h => h.HedefKayitlari)
                .OrderByDescending(h => h.OlusturmaTarihi)
                .ToList();

            // Son 7 gün kayıtları
            var son7Gun = DateTime.Today.AddDays(-6);
            var son7GunKayitlari = _context.HedefKayitlari
                .Include(hk => hk.Hedef)
                .Where(hk => hk.Hedef.KullaniciId == kullaniciId && hk.Tarih >= son7Gun)
                .OrderByDescending(hk => hk.Tarih)
                .ThenByDescending(hk => hk.Saat)
                .Take(10)
                .ToList();

            // Kullanıcının notları
            var notlar = _context.Notlar
                .Where(n => n.KullaniciId == kullaniciId)
                .OrderByDescending(n => n.OlusturmaTarihi)
                .Take(5)
                .ToList();

            // Rastgele öneri
            ViewBag.RastgeleOneri = GetRastgeleOneri();
            ViewBag.Son7GunKayitlari = son7GunKayitlari;
            ViewBag.Notlar = notlar;
            ViewBag.KullaniciId = kullaniciId;

            return View(hedefler);
        }

        // POST: Hedef/YeniHedef
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YeniHedef(HedefViewModel model)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            if (ModelState.IsValid)
            {
                var yeniHedef = new Hedef
                {
                    Baslik = model.Baslik,
                    Aciklama = model.Aciklama,
                    Periyot = model.Periyot,
                    OnemDerecesi = model.OnemDerecesi,
                    KullaniciId = kullaniciId.Value,
                    OlusturmaTarihi = DateTime.Now
                };

                _context.Hedefler.Add(yeniHedef);
                _context.SaveChanges();

                TempData["BasariMesaji"] = "Hedef başarıyla oluşturuldu!";
            }
            else
            {
                TempData["HataMesaji"] = "Hedef oluşturulurken hata oluştu. Lütfen tüm alanları kontrol edin.";
            }

            return RedirectToAction("Index");
        }

        // POST: Hedef/HedefSil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HedefSil(int id)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            var hedef = _context.Hedefler
                .Include(h => h.HedefKayitlari)
                .FirstOrDefault(h => h.Id == id && h.KullaniciId == kullaniciId);

            if (hedef != null)
            {
                // Hedefin kayıtları varsa tüm kayıtları da sil
                if (hedef.HedefKayitlari.Any())
                {
                    _context.HedefKayitlari.RemoveRange(hedef.HedefKayitlari);
                }

                _context.Hedefler.Remove(hedef);
                _context.SaveChanges();
                
                TempData["BasariMesaji"] = "Hedef ve ilgili tüm kayıtlar başarıyla silindi!";
            }
            else
            {
                TempData["HataMesaji"] = "Hedef bulunamadı veya silme yetkiniz yok.";
            }

            return RedirectToAction("Index");
        }

        // POST: Hedef/KayitEkle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KayitEkle(HedefKaydiViewModel model)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            if (ModelState.IsValid)
            {
                // Hedef kullanıcıya ait mi?
                var hedef = _context.Hedefler.FirstOrDefault(h => h.Id == model.HedefId && h.KullaniciId == kullaniciId);
                if (hedef == null)
                {
                    TempData["HataMesaji"] = "Geçersiz hedef.";
                    return RedirectToAction("Index");
                }

                var yeniKayit = new HedefKaydi
                {
                    HedefId = model.HedefId,
                    Tarih = model.Tarih,
                    Saat = model.Saat,
                    Sure = model.Sure,
                    Uygulandi = model.Uygulandi,
                    KayitTarihi = DateTime.Now
                };

                _context.HedefKayitlari.Add(yeniKayit);
                _context.SaveChanges();

                TempData["BasariMesaji"] = "Kayıt başarıyla eklendi!";
            }
            else
            {
                TempData["HataMesaji"] = "Kayıt eklenirken hata oluştu.";
            }

            return RedirectToAction("Index");
        }

        // POST: Hedef/NotEkle TODO:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NotEkle(string Aciklama, IFormFile? GorselDosya)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            if (string.IsNullOrEmpty(Aciklama))
            {
                TempData["HataMesaji"] = "Açıklama alanı zorunludur.";
                return RedirectToAction("Index");
            }

            var yeniNot = new Not
            {
                Aciklama = Aciklama,
                KullaniciId = kullaniciId.Value,
                OlusturmaTarihi = DateTime.Now,
                GorselYolu = ""
            };
            
            // Dosya upload 
            if (GorselDosya != null && GorselDosya.Length > 0)
            {
                // Dosya türü kontrolü
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(GorselDosya.FileName).ToLower();
                
                if (allowedExtensions.Contains(fileExtension))
                {
                    // Klasör oluşturma
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "notlar");
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    // Dosya adı oluşturma
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsPath, fileName);

                    // Dosyayı kaydetme
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        GorselDosya.CopyTo(stream);
                    }

                    yeniNot.GorselYolu = $"/uploads/notlar/{fileName}";
                }
                else
                {
                    TempData["HataMesaji"] = "Sadece .jpg, .jpeg, .png, .gif dosyaları yükleyebilirsiniz.";
                    return RedirectToAction("Index");
                }
            }

            _context.Notlar.Add(yeniNot);
            _context.SaveChanges();

            TempData["BasariMesaji"] = "Not başarıyla eklendi!";
            return RedirectToAction("Index");
        }

        // POST: Hedef/NotSil TODO:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NotSil(int id)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            var not = _context.Notlar.FirstOrDefault(n => n.Id == id && n.KullaniciId == kullaniciId);
            if (not != null)
            {
                // Dosya varsa sil
                if (!string.IsNullOrEmpty(not.GorselYolu))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", not.GorselYolu.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Notlar.Remove(not);
                _context.SaveChanges();
                TempData["BasariMesaji"] = "Not başarıyla silindi!";
            }
            else
            {
                TempData["HataMesaji"] = "Not bulunamadı veya silme yetkiniz yok.";
            }

            return RedirectToAction("Index");
        }

        // GET: Hedef/HedefKayitKontrol TODO:
        public JsonResult HedefKayitKontrol(int id)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return Json(new { kayitSayisi = 0 });
            }

            var kayitSayisi = _context.HedefKayitlari
                .Count(hk => hk.HedefId == id && hk.Hedef.KullaniciId == kullaniciId);

            return Json(new { kayitSayisi = kayitSayisi });
        }

        private string GetRastgeleOneri()
        {
            var aktifOneriler = _context.Oneriler.Where(o => o.Aktif).ToList();
            if (aktifOneriler.Any())
            {
                var random = new Random();
                var rastgeleIndex = random.Next(aktifOneriler.Count);
                return aktifOneriler[rastgeleIndex].Metin;
            }
            return "Ağız ve diş sağlığınızı ihmal etmeyin!";
        }
    }
}