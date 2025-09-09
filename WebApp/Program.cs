using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Repositories;
using WebApp.Models.Entities;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionStringMyContext = builder.Configuration.GetConnectionString("CinePlusDb") ?? throw new InvalidOperationException("Connection string 'CinePlusDb' not found.");

builder.Services.AddDbContext<MyContext>(options => options.UseSqlServer(connectionStringMyContext));

// Injection de dépendances
// REPOSITORIES
builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
builder.Services.AddScoped<ISalleRepository, SalleRepository>();
builder.Services.AddScoped<IFilmRepository, FilmRepository>();
builder.Services.AddScoped<ISeanceRepository, SeanceRepository>();
builder.Services.AddScoped<IHoraireRepository, HoraireRepository>();
// Injection de dépendances
// SERVICES
builder.Services.AddScoped<ICinemaService, CinemaService>();
builder.Services.AddScoped<ISalleService, SalleService>();
builder.Services.AddScoped<IFilmService, FilmService>();
builder.Services.AddScoped<ISeanceService, SeanceService>();
builder.Services.AddScoped<IHoraireService, HoraireService>();
builder.Services.AddDefaultIdentity<Admin>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<MyContext>();
builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();