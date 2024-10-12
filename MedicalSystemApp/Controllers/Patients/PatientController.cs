using MedicalSystem.Application.Interfaces.Services.Appoint;
using MedicalSystem.Application.Interfaces.Services.Generic;
using MedicalSystem.Application.Interfaces.Services.Patients;
using MedicalSystem.Application.Interfaces.Services.UserS;
using MedicalSystem.Application.ViewModel.Patient;
using MedicalSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MedicalSystemApp.Controllers.Patients
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IService<Clinic> _clinicService;
        private readonly IUserService _userService;
        private readonly IAppointmentService _appointmentService;

        public PatientController(IPatientService patientService, IService<Clinic> clinicService, IUserService userService, IAppointmentService appointmentService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
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

            var clinicId = await _userService.GetClinicIdByUserIdAsync(userId);

            var patients = await _patientService.GetPatientByClinicAsync(clinicId);

            return View(patients);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PatientViewModel
            {
                Clinics = await GetClinicsAsync()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel model, IFormFile photo)
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

                model.ClinicId = clinicId;

                if (photo != null && photo.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                    var filePath = Path.Combine("wwwroot/images/patients", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    model.Photo = fileName;
                }

                bool patientRegistered = await _patientService.CreatePatientAsync(model);
                if (!patientRegistered)
                {
                    ModelState.AddModelError(string.Empty, "El paciente ya existe");
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
            var patient = await _patientService.GetPatientAsync(id);
            if (patient == null) return NotFound();

            var model = new PatientViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                Phone = patient.Phone,
                IDCard = patient.IDCard,
                Photo = patient.Photo,
                ClinicId = patient.ClinicId,
                Clinics = await GetClinicsAsync()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(PatientViewModel model, IFormFile? photoFile)
        {
            if (!ModelState.IsValid)
            {
                model.Clinics = await GetClinicsAsync();
                return View(model);
            }

            try
            {
                if (photoFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
                    var filePath = Path.Combine("wwwroot/images/patients", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(stream);
                    }
                    model.Photo = fileName;
                }
                else
                {
                    var existingPatient = await _patientService.GetPatientAsync(model.Id);
                    model.Photo = existingPatient.Photo;
                }

                bool patientUpdated = await _patientService.UpdatePatientAsync(model);
                if (!patientUpdated)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar el paciente");
                    model.Clinics = await GetClinicsAsync();
                    return View(model);
                }

                return RedirectToAction("Index", new { clinicId = model.ClinicId });
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
            var hasAppointment = await _appointmentService.PatientHasAppointmentsAsync(id);
            if (hasAppointment)
            {
                ModelState.AddModelError("", "El paciente no se puede eliminar porque tiene citas asociadas.");
                return RedirectToAction("Index");
            }
            var result = await _patientService.DeletePatientAsync(id);
            if(!result) return NotFound();

            return RedirectToAction("Index");
        }
        private async Task<IEnumerable<SelectListItem>> GetClinicsAsync()
        {
            var clinics = await _clinicService.GetAllAsync();
            return clinics.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            }).ToList();
        }
    }

}
