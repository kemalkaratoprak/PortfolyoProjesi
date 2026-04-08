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
                // 1. Veritabanına Kaydetme İşlemi
                _context.ContactMessages.Add(model);
                await _context.SaveChangesAsync();

                // 2. Mail Gönderme İşlemi
                try
                {
                    var mail = new MailMessage();
                    mail.From = new MailAddress("kemalkaratoprakk@gmail.com"); // Buraya kendi Gmail adresini yaz
                    mail.To.Add("kemalxkaratoprak@gmail.com"); // Mesajın gideceği adres
                    mail.Subject = "Portfolyo Yeni Mesaj: " + model.Subject;
                    mail.Body = $"Gönderen: {model.Name} <br> E-posta: {model.Email} <br><br> Mesaj: {model.Message}";
                    mail.IsBodyHtml = true;

                    using (SmtpClient sc = new SmtpClient("smtp.gmail.com", 587))
                    {
                        sc.EnableSsl = true;
                        sc.Credentials = new NetworkCredential("kemalkaratoprakk@gmail.com", "ufinjwesmgssxlf"); // Buraya 16 haneli uygulama şifreni yaz
                        sc.Timeout = 10000; // 10 saniye bekler, sonra hata verir (Sonsuz yüklenmeyi engeller)

                        await sc.SendMailAsync(mail);
                    }
                }
                catch (Exception ex)
                {
                    // Mail gitmese bile mesaj veritabanında saklı, bu yüzden hata vermeden ana sayfaya döner
                    TempData["MessageSent"] = "Mesajınız kaydedildi ancak mail iletilemedi.";
                    return RedirectToAction("Index");
                }

                TempData["MessageSent"] = "Mesajınız başarıyla gönderildi!";
            }

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
    }
}