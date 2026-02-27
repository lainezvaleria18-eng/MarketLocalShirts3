using MarketLocalShirts3.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MarketLocalShirts3Context>(options =>
    options.UseSqlServer("Server=.;Database=MarketLocalShirts3;Trusted_Connection=True;TrustServerCertificate=True"));

builder.Services.AddSession();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cliente}/{action=Catalogo}/{id?}");

app.Run();