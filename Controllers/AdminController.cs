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

        // --- YENİ EKLENEN MESAJ YÖNETİM METODLARI ---

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
            ViewBag.Error = "Hatalı giriş!";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}