using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolyoProjesi.Models; // Modellerimizi kullanabilmek için

namespace PortfolyoProjesi.Controllers
{
    public class HomeController : Controller
    {
        // Köprümüzü (AppDbContext) burada tanımlıyoruz
        private readonly AppDbContext _context;

        // Controller çalıştığında köprümüz otomatik olarak buraya gelecek
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Ana sayfa (Index) açıldığında çalışacak metod
        public IActionResult Index()
        {
            // Veri tabanındaki 'Projects' tablosundaki tüm verileri bir listeye çevirip alıyoruz
            var projects = _context.Projects.ToList();
            
            // Bu listeyi, ekranda çizilmesi için View'a (HTML kısmına) gönderiyoruz
            return View(projects);
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
    
        // 1. Boş formu ekrana getiren metod
        public IActionResult Create()
        {
            return View();
        }

        // 2. Formdan gelen veriyi yakalayıp veri tabanına kaydeden metod
        [HttpPost]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Add(project); // Yeni projeyi listeye ekle
                _context.SaveChanges(); // SQL'e gönder ve kaydet
                return RedirectToAction("Index"); // İşlem bitince ana sayfaya dön
            }
            return View(project); // Eğer formda hata varsa, tekrar formu göster
        }
        // Projeyi Silme İşlemi
public IActionResult Delete(int id)
{
    var project = _context.Projects.Find(id); // Silinecek projeyi ID ile bul
    if (project != null)
    {
        _context.Projects.Remove(project); // Listeden kaldır
        _context.SaveChanges(); // Veri tabanından tamamen sil
    }
    return RedirectToAction("Index"); // Ana sayfaya dön
}
        // 1. Düzenleme formunu, seçilen projenin bilgileriyle dolu olarak açar
public IActionResult Edit(int id)
{
    var project = _context.Projects.Find(id);
    if (project == null) return NotFound();
    return View(project);
}

// 2. Formdan gelen güncel bilgileri veri tabanına kaydeder
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
    }

}