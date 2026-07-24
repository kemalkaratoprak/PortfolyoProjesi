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
        public IActionResult SendMessage(ContactMessage model, string? CatchSpamTrap) // DİKKAT: string yanına soru işareti (?) koyduk
        {
            // 1. KİM ENGELLİYOR: HONEYPOT (BOT KORUMASI) MU?
            if (!string.IsNullOrEmpty(CatchSpamTrap))
            {
                TempData["MessageSent"] = "HATA: Tarayıcın gizli alanı doldurduğu için sistem seni Spam Bot sandı!";
                return RedirectToAction("Index", "Home"); 
            }

            // 2. GÜVENLİK DUVARI (MODELSTATE) TEMİZLİĞİ
            // Doğrulamadan muaf tuttuğumuz alanlar
            ModelState.Remove("Id");
            ModelState.Remove("CreatedDate");
            ModelState.Remove("IsRead");
            ModelState.Remove("CatchSpamTrap"); // Honeypot'u da doğrulama dışı bırakıyoruz ki zorunlu sanmasın

            // 3. NORMAL KAYIT İŞLEMİ 
            if (ModelState.IsValid)
            {
                _context.ContactMessages.Add(model);
                _context.SaveChanges();
                TempData["MessageSent"] = "Mesajınız başarıyla gönderildi."; // Başarılı mesajını eski haline getirdik
            }
            else
            {
                // Ne olur ne olmaz, başka bir eksik varsa yine ekranda görelim
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["MessageSent"] = "C# HATASI: " + string.Join(" | ", errors);
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