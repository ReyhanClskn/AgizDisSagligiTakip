using Microsoft.AspNetCore.Mvc;
using AgizDisSagligiTakip.Web.Models;
using AgizDisSagligiTakip.Data.Context;
using System.Diagnostics;

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
            // Giriş yapmış kullanıcı için rastgele öneri getir
            if (HttpContext.Session.GetInt32("KullaniciId") != null)
            {
                var rastgeleOneri = GetRastgeleOneri();
                ViewBag.RastgeleOneri = rastgeleOneri;
                
                // Son 7 gün verileri (şimdilik boş, sonra ekleyeceğim)
                ViewBag.Son7GunVerileri = "Henüz veri bulunmuyor.";
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