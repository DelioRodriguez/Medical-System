using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.Interfaces.Services.UserS;
using MedicalSystem.Application.ViewModel.MantUser;
using MedicalSystem.Application.ViewModel.User;
using MedicalSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MedicalSystemApp.Controllers.MantUser
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IService<Clinic> _clinicService;

        public UserController(IUserService userService, IService<Clinic> clinicService)
        {
            _userService = userService;
            _clinicService = clinicService;
        }

     
        public async Task<IActionResult> Index()
        {
           
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(); 
            }

            var userId = int.Parse(userIdClaim);

            var clinicId = await _userService.GetClinicIdByUserIdAsync(userId);

            var users = await _userService.GetUsersByClinicAsync(clinicId);
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new RegisterViewModel
            {
                Clinics = await GetClinicsAsync(),
                Roles = GetRoles()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = GetRoles(); 
                return View(model);
            }

            try
            {
              
                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var clinicId = await _userService.GetClinicIdByUserIdAsync(loggedInUserId);

              
                model.ClinicID = clinicId;
                bool userRegistered = await _userService.RegisterUserAsync(model, loggedInUserId);

                if (!userRegistered)
                {
                    ModelState.AddModelError(string.Empty, "El nombre de usuario ya existe.");
                    model.Roles = GetRoles();
                    return View(model);
                }

                return RedirectToAction("Index", new { clinicId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Roles = GetRoles();
                return View(model);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirsName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                Role = user.Role,

            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Clinics = await GetClinicsAsync();
                model.Roles = GetRoles();
                return View(model);
            }

            try
            {

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    model.Password = null;
                }
                var currentUser = await _userService.GetByIdAsync(model.Id);
                model.Role = currentUser.Role;

                bool userUpdated = await _userService.UpdateUserAsync(model);

                if (!userUpdated)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar el usuario.");
                    model.Clinics = await GetClinicsAsync();
                    model.Roles = GetRoles();
                    return View(model);
                }

                return RedirectToAction("Index", new { clinicId = model.ClinicID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Clinics = await GetClinicsAsync();
                model.Roles = GetRoles();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            
            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (id == loggedInUserId)
            {
                ModelState.AddModelError(string.Empty, "No puedes eliminar tu propia cuenta.");
                return RedirectToAction("Index");
            }

         
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
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


        private List<SelectListItem> GetRoles()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Administrador", Text = "Administrador" },
                new SelectListItem { Value = "Asistente", Text = "Asistente" }
            };
        }
    }
}
