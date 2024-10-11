using MedicalSystem.Application.Services.AuthService;
using MedicalSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Application.Services.Generic;
using MedicalSystem.Infrastructure.Persistence.Repositories.Generic;
using MedicalSystem.Application.Interfaces.Repository.Auth;
using MedicalSystem.Application.Interfaces.Services.Auth;
using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.Interfaces.Repository.Generic;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Application.Interfaces.Services.UserS;
using MedicalSystem.Application.Interfaces.Repository.UserRepository;
using MedicalSystem.Infrastructure.Persistence.Repositories.UserRepo;
using Microsoft.AspNetCore.Authentication.Cookies;
using MedicalSystem.Application.Interfaces.Services.DoctorSe;
using MedicalSystem.Application.Services.DoctorSer;
using MedicalSystem.Application.Interfaces.Repository.DoctorRe;
using MedicalSystem.Infrastructure.Persistence.Repositories.DoctorRepo;
using MedicalSystem.Application.Services.UserSer;
using MedicalSystem.Application.Interfaces.Services.Patients;
using MedicalSystem.Application.Services.PatientSe;
using MedicalSystem.Application.Interfaces.Repository.Patients;
using MedicalSystem.Infrastructure.Persistence.Repositories.PatientsRe;
using MedicalSystem.Application.Interfaces.Repository.AppointmentS;
using MedicalSystem.Application.Interfaces.Services.Appoint;

using MedicalSystem.Infrastructure.Persistence.Repositories.AppointmentRep;
using MedicalSystem.Application.Interfaces.Repository.Test;
using MedicalSystem.Application.Interfaces.Services.Test;
using MedicalSystem.Application.Services.Test;
using MedicalSystem.Infrastructure.Persistence.Repositories.Test;
using MedicalSystem.Application.Interfaces.Repository.Lab;
using MedicalSystem.Application.Interfaces.Services.Lab;
using MedicalSystem.Application.Services.Lab;
using MedicalSystem.Infrastructure.Persistence.Repositories.Lab;
using MedicalSystem.Application.Services.AppointmentSer;
using MedicalSystem.Application;
using MedicalSystem.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
