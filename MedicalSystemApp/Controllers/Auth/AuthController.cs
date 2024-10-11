using MedicalSystem.Application.Interfaces.Services.Auth;
using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.ViewModel.User;
using MedicalSystem.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using MedicalSystem.Application.Interfaces.Services.UserS;

namespace MedicalSystemApp.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IService<Clinic> _clinicService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IService<Clinic> clinicService, IUserService user)
        {
            _authService = authService;
            _clinicService = clinicService;
            _userService = user;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userService.ValidateUserAsync(model.username, model.password);
            if (user == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View(model);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (user.Role == "Administrador")
            {
                return RedirectToAction("Index", "Home"); 
            }
           

            // Si el rol no es ninguno de los anteriores, puedes redirigir a una acción por defecto
            return RedirectToAction("Index", "Asits");
        }



        // GET: /Auth/Register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel
            {
                Clinics = await GetClinicsAsync(),
                Roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "Administrador", Text = "Administrador" }
        }
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Clinics = await GetClinicsAsync(); // Puede que no sea necesario si no se muestra la selección.
                return View(model);
            }

            try
            {
                // Crear la clínica
                var clinic = new Clinic
                {
                    Name = model.ClinicName
                };

                await _clinicService.addAsync(clinic); // Asegúrate de tener este método en tu IService

                // Obtener el ID de la clínica recién creada
                model.ClinicID = clinic.Id; // Asignar el ID de la clínica recién creada

                // Asignar un rol predeterminado
                model.Role = "Administrador"; // Cambia "Administrador" si necesitas un rol diferente

                // Luego, registra al usuario
                await _authService.RegisterUser(model);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Clinics = await GetClinicsAsync(); // Puede que no sea necesario si no se muestra la selección.
                return View(model);
            }
        }



        [HttpGet]
        public IActionResult LogoutConfirmation()
        {
            return View();
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth"); // Redirige a la página de inicio de sesión
        }
        private async Task<IEnumerable<SelectListItem>> GetClinicsAsync()
        {
            var clinics = await _clinicService.GetAllAsync(); 
            return clinics.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }

    }
}
