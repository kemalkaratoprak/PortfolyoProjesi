using Microsoft.EntityFrameworkCore;
using PortfolyoProjesi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. ADIM: Güvenlik Servislerini (Cookie) Tanımlıyoruz
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyCookieAuth";
        options.LoginPath = "/Admin/Login"; // Giriş yapmayanları bu sayfaya yolla
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veri tabanı köprümüzü (DbContext) sisteme dahil ediyoruz
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Statik dosyaları (CSS/JS) kullanıma aç

app.UseRouting();

// 2. ADIM: Kimlik Kontrolü Sıralaması (Çok Önemli!)
app.UseAuthentication(); // 1. Sen kimsin? (Giriş yapıldı mı?)
app.UseAuthorization();  // 2. Buraya girmeye iznin var mı?

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();