using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolyoProjesi.Models;
using System.Net; // Mail için gerekli
using System.Net.Mail; // Mail için gerekli

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
                try
                {
                    // 1. Önce veritabanına kaydedelim (Manyetik yedek)
                    _context.ContactMessages.Add(model);
                    await _context.SaveChangesAsync();

                    // 2. Şimdi sana e-posta gönderelim
                    var sc = new SmtpClient();
                    sc.Port = 587;
                    sc.Host = "smtp.gmail.com";
                    sc.EnableSsl = true;
                    sc.Credentials = new NetworkCredential("kemalkaratoprakk@gmail.com", "nnyb okyx xglt lrbr");

                    var mail = new MailMessage();
                    mail.From = new MailAddress("kemalkaratoprakk@gmail.com", "Portfolyo Bildirim");
                    mail.To.Add("kemalxkaratopak@gmail.com"); // Buraya mesajın gelmesini istediğin maili yaz
                    mail.Subject = "Siteden Yeni Mesaj: " + model.Subject;
                    mail.IsBodyHtml = true;
                    mail.Body = $@"
                        <h3>Yeni Bir İletişim Mesajı Alındı</h3>
                        <hr>
                        <p><b>Gönderen:</b> {model.Name} ({model.Email})</p>
                        <p><b>Konu:</b> {model.Subject}</p>
                        <p><b>Mesaj:</b> {model.Message}</p>";

                    await sc.SendMailAsync(mail);
                    
                    TempData["MessageSent"] = "Mesajınız başarıyla gönderildi!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Mesaj gönderilirken bir hata oluştu.";
                }
            }
            return RedirectToAction("Index");
        }

        // --- PROJE İŞLEMLERİ (MEVCUT KODLARIN) ---
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