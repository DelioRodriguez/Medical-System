
using MedicalSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.Cookies;
using MedicalSystem.Application;
using MedicalSystem.Infrastructure.Persistence;
using MedicalSystem.Application.Services.Generic;
using MedicalSystem.Application.Interfaces.Services.Generic;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
ServiceRegistration.RegisterServices(builder.Services);
RepositoryRegistration.RegisterRepositories(builder.Services);



// Configuración de autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";  
        options.LogoutPath = "/Auth/Logout"; 
        options.AccessDeniedPath = "/Auth/AccessDenied"; 
    });

// Construir la aplicación
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage(); // Página de error en desarrollo
}

app.UseStaticFiles();
app.UseRouting();

// Usar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
