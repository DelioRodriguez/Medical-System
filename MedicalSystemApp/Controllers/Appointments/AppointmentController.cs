using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MedicalSystem.Application.ViewModel.Ammoin;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicalSystem.Application.ViewModel.Lab;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Application.ViewModel.Extras;
using MedicalSystem.Application.Interfaces.Services.Appoint;
using MedicalSystem.Application.Interfaces.Services.DoctorSe;
using MedicalSystem.Application.Interfaces.Services.Lab;
using MedicalSystem.Application.Interfaces.Services.Patients;
using MedicalSystem.Application.Interfaces.Services.Test;
using MedicalSystem.Application.Interfaces.Services.UserS;

namespace MedicalSystemApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IUserService _userService;
        private readonly ILabTestService _labTestService;
        private readonly ILabResultService _labResultService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IPatientService patientService,
            IUserService userService,
            ILabTestService labTestService,
            ILabResultService labResultService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _patientService = patientService;
            _userService = userService;
            _labTestService = labTestService;
            _labResultService = labResultService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var clinicId = await _userService.GetClinicIdByUserIdAsync(userId);
            var appointments = await _appointmentService.GetAppointmentsAsync(clinicId);
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AppointmentViewModel
            {
                Doctors = (List<SelectListItem>)await GetDoctorsAsync(),
                Patients = (List<SelectListItem>)await GetPatientsAsync(),
                ClinicId = await GetClinicIdAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Doctors = (List<SelectListItem>)await GetDoctorsAsync();
                model.Patients = (List<SelectListItem>)await GetPatientsAsync();
                return View(model);
            }

            await _appointmentService.CreateAppointmentAsync(model);
            return RedirectToAction(nameof(Index));
        }

        private async Task<int> GetClinicIdAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await _userService.GetClinicIdByUserIdAsync(userId);
        }

        private async Task<IEnumerable<SelectListItem>> GetDoctorsAsync()
        {
            var doctors = await _doctorService.GetDoctorByClinicAsync(await GetClinicIdAsync());
            return doctors.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.Firstname} {d.Lastname}"
            }).ToList();
        }

        private async Task<IEnumerable<SelectListItem>> GetPatientsAsync()
        {
            var patients = await _patientService.GetPatientByClinicAsync(await GetClinicIdAsync());
            return patients.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.FirstName} {p.LastName}"
            }).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Consult(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var labTests = await _labTestService.GetLabTestsByClinicAsync(appointment.ClinicId);

            var model = new ConsultLabTestsViewModel
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                ClinicId = appointment.ClinicId,
                LabTests = labTests
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLabResults(int appointmentId, int patientId, int[] labTestIds)
        {
            foreach (var labTestId in labTestIds)
            {
                var createResultViewModel = new CreateResultViewModel
                {
                    PatientId = patientId,
                    LabTestId = labTestId,
                    Result = null,
                    Status = Status.PENDIENTEDERESULTADOS,
                    ClinicId = await GetClinicIdAsync(),
                    AppointmentId = appointmentId
                };

                await _labResultService.CreateLabResultAsync(createResultViewModel);
            }

            await _appointmentService.UpdateAppointmentStatusToPendingResultsAsync(appointmentId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ViewResults(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetByIdAsync(id);

                if (appointment == null)
                {
                    return NotFound();
                }

                var clinicId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (clinicId is null) throw new InvalidOperationException("No se encontró el ClinicId en los Claims");

                int ClinicID = int.Parse(clinicId);
                var cliniID = await _userService.GetClinicIdByUserIdAsync(ClinicID);

                var labResults = await _labResultService.GetAllConfirmedResultsByAppointmentIdAsync(cliniID, id);

                var viewModel = new LabResultViewModel
                {
                    AppointmentId = appointment.Id,
                    PatientName = appointment.Patient.FirstName,
                    LabResults = labResults.Select(r => new LabResultDetailViewModel
                    {
                        PatientFullName = r.Patient.FirstName,
                        TestName = r.LabTest.Name,
                        Result = r.Result
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new LabResultViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteAppointment(int appointmentId)
        {
            var appointment = await _appointmentService.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = Status.COMPLETADA;
            await _appointmentService.UpdateAsync(appointment);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewCompletedResults(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetByIdAsync(id);

                if (appointment == null)
                {
                    return NotFound();
                }

                var clinicId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (clinicId is null) throw new InvalidOperationException("No se encontró el ClinicId en los Claims");

                int ClinicID = int.Parse(clinicId);
                var cliniID = await _userService.GetClinicIdByUserIdAsync(ClinicID);

                var labResults = await _labResultService.GetAllConfirmedResultsByAppointmentIdAsync(cliniID, id);

                var viewModel = new LabResultViewModel
                {
                    AppointmentId = appointment.Id,
                    PatientName = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}",
                    LabResults = labResults.Select(r => new LabResultDetailViewModel
                    {
                        PatientFullName = $"{r.Patient.FirstName} {r.Patient.LastName}",
                        TestName = r.LabTest.Name,
                        Result = r.Result
                    }).ToList()
                };

                return View("ViewCompletedResults", viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new LabResultViewModel());
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            await _appointmentService.DeleteAppointmentAsync(id);
            return RedirectToAction(nameof(Index)); 
        }


    }
}
