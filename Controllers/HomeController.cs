using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolyoProjesi.Models;
using System.Net; 
using System.Net.Mail; 

namespace PortfolyoProjesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var projects = _context.Projects.ToList();
            return View(projects);
        }

        // --- İLETİŞİM FORMU VE MAİL GÖNDERME METODU ---
                [HttpPost]
        public async Task<IActionResult> SendMessage(ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                // 1. Sadece Veritabanına Kaydediyoruz
                _context.ContactMessages.Add(model);
                await _context.SaveChangesAsync();

                // 2. Kullanıcıya başarı mesajı veriyoruz
                TempData["MessageSent"] = "Mesajınız başarıyla iletildi!";
                
                // 3. Hiç beklemeden ana sayfaya dönüyoruz
                return RedirectToAction("Index");
            }

            // Model geçerli değilse sayfaya geri dön
            return RedirectToAction("Index");
        }

        // --- PROJE İŞLEMLERİ ---
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Add(project);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public IActionResult Delete(int id)
        {
            var project = _context.Projects.Find(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        public IActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Update(project);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
        // --- SADECE SENİN GÖREBİLECEĞİN MESAJ LİSTESİ ---
                public IActionResult AdminMessages()
        {
            // Veritabanındaki ContactMessages tablosunun tamamını listeye çevirip getirir.
            // OrderByDescending(m => m.Id) sayesinde ID'si en büyük olan (en yeni) mesaj en üstte olur.
            var messages = _context.ContactMessages
                                .OrderByDescending(m => m.Id)
                                .ToList(); 

            return View(messages);
        }
    }
}