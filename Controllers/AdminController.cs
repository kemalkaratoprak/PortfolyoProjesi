using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PortfolyoProjesi.Models;

namespace PortfolyoProjesi.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(AuthenticationSchemes = "MyCookieAuth")]
        public IActionResult Index()
        {
            var projects = _context.Projects.ToList();
            // Mesajları tarihe göre (en yeni en üstte) çekiyoruz
            ViewBag.Messages = _context.ContactMessages.OrderByDescending(m => m.CreatedDate).ToList();
            return View(projects);
        }

        // --- MESAJ YÖNETİM METODLARI ---

        [Authorize(AuthenticationSchemes = "MyCookieAuth")]
        public IActionResult DeleteMessage(int id)
        {
            var message = _context.ContactMessages.Find(id);
            if (message != null)
            {
                _context.ContactMessages.Remove(message);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "MyCookieAuth")]
        public IActionResult MarkAsRead(int id)
        {
            var message = _context.ContactMessages.Find(id);
            if (message != null)
            {
                message.IsRead = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // --- GİRİŞ / ÇIKIŞ İŞLEMLERİ ---

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Değişkenleri Render panelindeki Environment Variables kısmından çekiyoruz
            var adminUser = Environment.GetEnvironmentVariable("ADMIN_USER");
            var adminPass = Environment.GetEnvironmentVariable("ADMIN_PASS");

            // Güvenlik Kontrolü: Bilgiler eşleşiyor mu ve sistem değişkenleri boş değil mi?
            if (!string.IsNullOrEmpty(adminUser) && username == adminUser && password == adminPass)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Tarayıcı kapatılsa da oturum hatırlanır
                };

                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Admin");
            }

            // Hata durumunda mesaj gönder
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}