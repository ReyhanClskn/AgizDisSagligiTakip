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
                // Hedef kayıtları varsa kullanıcı onayı kontrolü TODO: frontend
                _context.Hedefler.Remove(hedef);
                _context.SaveChanges();
                TempData["BasariMesaji"] = "Hedef başarıyla silindi!";
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

        // POST: Hedef/NotEkle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NotEkle(NotViewModel model)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            if (ModelState.IsValid)
            {
                var yeniNot = new Not
                {
                    Aciklama = model.Aciklama,
                    KullaniciId = kullaniciId.Value,
                    OlusturmaTarihi = DateTime.Now,
                    GorselYolu = "" // Şimdilik boş TODO:
                };

                _context.Notlar.Add(yeniNot);
                _context.SaveChanges();

                TempData["BasariMesaji"] = "Not başarıyla eklendi!";
            }
            else
            {
                TempData["HataMesaji"] = "Not eklenirken hata oluştu.";
            }

            return RedirectToAction("Index");
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