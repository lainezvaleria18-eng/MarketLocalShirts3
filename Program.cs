using MarketLocalShirts3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
//codigos que hay que tener cuidado par

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MarketLocalShirts3Context>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MarketLocalShirts3;Trusted_Connection=True;MultipleActiveResultSets=true"
    ));

builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cliente/Login";
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cliente}/{action=Inicio}/{id?}");

app.Run();