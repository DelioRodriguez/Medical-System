using MedicalSystem.Application.Interfaces.Services.DoctorSe;
using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.Interfaces.Services.UserS;
using MedicalSystem.Application.ViewModel.Doctor;
using MedicalSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MedicalSystemApp.Controllers.Docto
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IService<Clinic> _clinicService;
        private readonly IUserService _userService;

        public DoctorController(IDoctorService doctorService, IService<Clinic> clinicService, IUserService userService)
        {
            _doctorService = doctorService;
            _clinicService = clinicService;
            _userService = userService;
        }


        public async Task<IActionResult> Index()
        {

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim);


            var clinicId = await _doctorService.GetClinicByDoctorIdAsync(userId);


            var doctors = await _doctorService.GetDoctorByClinicAsync(clinicId);
            return View(doctors);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new DoctorViewModel
            {
                Clinics = await GetClinicsAsync()
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel model, IFormFile photo)
        {
            if (!ModelState.IsValid)
            {
                model.Clinics = await GetClinicsAsync();
                return View(model);
            }

            try
            {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var clinicId = await _userService.GetClinicIdByUserIdAsync(int.Parse(userId));

                model.ClinicID = clinicId;

                if (photo != null && photo.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                    var filePath = Path.Combine("wwwroot/images/doctors", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    model.Photo = fileName;
                }

                bool doctorRegistered = await _doctorService.CreateDoctorAsync(model);

                if (!doctorRegistered)
                {
                    ModelState.AddModelError(string.Empty, "El médico ya existe.");
                    model.Clinics = await GetClinicsAsync();
                    return View(model);
                }

                return RedirectToAction("Index", new { clinicId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Clinics = await GetClinicsAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _doctorService.GetDoctorAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            var model = new DoctorViewModel
            {
                Id = doctor.Id,
                Firstname = doctor.Firstname,
                Lastname = doctor.Lastname,
                Email = doctor.Email,
                Phone = doctor.Phone,
                IDCard = doctor.IDCard,
                Photo = doctor.Photo,
                ClinicID = doctor.ClinicID,
                Clinics = await GetClinicsAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorViewModel model, IFormFile? photoFile)
        {
            if (!ModelState.IsValid)
            {
                model.Clinics = await GetClinicsAsync();
                return View(model);
            }

            try
            {

                if (photoFile == null)
                {

                    var existingDoctor = await _doctorService.GetDoctorAsync(model.Id);
                    model.Photo = existingDoctor.Photo;
                }
                else
                {

                    var filePath = Path.Combine("wwwroot/images/doctors", photoFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(stream);
                    }
                    model.Photo = photoFile.FileName;
                }

                bool doctorUpdated = await _doctorService.UpdateDoctorAsync(model);

                if (!doctorUpdated)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar el médico.");
                    model.Clinics = await GetClinicsAsync();
                    return View(model);
                }

                return RedirectToAction("Index", new { clinicId = model.ClinicID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Clinics = await GetClinicsAsync();
                return View(model);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _doctorService.DeleteDoctorAsync(id);
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
    }
}
