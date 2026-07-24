using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolyoProjesi.Models;


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
            var counter = _context.VisitorCounters.FirstOrDefault();
            if (counter != null)
            {
                counter.TotalVisits++;
                _context.SaveChanges();
                // Sadece artırıp kaydediyoruz, ViewBag kısmını sildik.
            }

            var projects = _context.Projects.ToList();
            return View(projects);
        }

        [HttpPost]
        public IActionResult SendMessage(ContactMessage model, string CatchSpamTrap)
        {
            // 1. HONEYPOT KONTROLÜ: Anlamsız isimli alan doluysa, bu kesinlikle bir bottur.
            if (!string.IsNullOrEmpty(CatchSpamTrap))
            {
                TempData["MessageSent"] = "Mesajınız başarıyla gönderildi.";
                return RedirectToAction("Index", "Home"); 
            }

            // 2. GÜVENLİK DUVARI (MODELSTATE) TEMİZLİĞİ
            // Ziyaretçinin formda doldurmayacağı, sistemin otomatik atadığı alanları doğrulamadan çıkarıyoruz.
            ModelState.Remove("Id");
            ModelState.Remove("CreatedDate");
            ModelState.Remove("IsRead");

            // 3. NORMAL KAYIT İŞLEMİ 
            if (ModelState.IsValid)
            {
                _context.ContactMessages.Add(model);
                _context.SaveChanges();
                TempData["MessageSent"] = "Mesajınız başarıyla gönderildi.";
            }

            return RedirectToAction("Index", "Home");
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
    
 
    }
}