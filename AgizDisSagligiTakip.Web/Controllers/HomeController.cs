using Microsoft.AspNetCore.Mvc;
using AgizDisSagligiTakip.Web.Models;
using AgizDisSagligiTakip.Data.Context;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace AgizDisSagligiTakip.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UygulamaDbContext _context;

        public HomeController(ILogger<HomeController> logger, UygulamaDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Giriş yapmış kullanıcı için verileri getir
            if (HttpContext.Session.GetInt32("KullaniciId") != null)
            {
                var kullaniciIdNullable = HttpContext.Session.GetInt32("KullaniciId");
                if (kullaniciIdNullable == null) return View();

                var kullaniciId = kullaniciIdNullable.Value;
                
                // Rastgele öneri
                var rastgeleOneri = GetRastgeleOneri();
                ViewBag.RastgeleOneri = rastgeleOneri;
                
                // Son 7 gün kayıtları
                var son7Gun = DateTime.Today.AddDays(-6);
                
                var son7GunKayitlari = _context.HedefKayitlari
                    .Include(hk => hk.Hedef) 
                    .Where(hk => hk.Hedef.KullaniciId == kullaniciId && hk.Tarih >= son7Gun)
                    .OrderByDescending(hk => hk.Tarih)
                    .ThenByDescending(hk => hk.Saat)
                    .Take(5) // Son 5 kayıt
                    .ToList();
                
                ViewBag.Son7GunKayitlari = son7GunKayitlari;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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