using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PortfolyoProjesi.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolyoProjesi.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // --- GÜVENLİ ALAN ---
        // Sadece giriş yapanlar bu listeyi görebilir
                    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
        [Authorize(AuthenticationSchemes = "MyCookieAuth")]
        public IActionResult Index()
        {
            // Projeleri çekiyoruz
            var projects = _context.Projects.ToList();

            // Mesajları çekiyoruz 
            ViewBag.Messages = _context.ContactMessages.OrderByDescending(m => m.Id).ToList();

            return View(projects);
        }

        // --- GİRİŞ İŞLEMLERİ ---

        // 1. Giriş Sayfasını Göster (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. Şifreyi Kontrol Et (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // ŞİMDİLİK BASİT ŞİFRE KONTROLÜ (Kendine göre değiştirebilirsin)
            if (username == "admin" && password == "Kemal123!")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Admin");
            }

            // Şifre yanlışsa uyarı ver
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // 3. Güvenli Çıkış (Logout)
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}