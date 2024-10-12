using MedicalSystem.Application.Interfaces.Services.Lab;
using MedicalSystem.Application.Interfaces.Services.UserS;
using MedicalSystem.Application.ViewModel.Lab;

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace MedicalSystemApp.Controllers.Lab
{
    public class LabResultController : Controller
    {
        private readonly ILabResultService _resultService;
        private readonly IUserService _userService;

        public LabResultController(ILabResultService labResultService, IUserService userService)
        {
            _resultService = labResultService;
            _userService = userService;
        }

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
               var clinicId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (clinicId is null) throw new InvalidOperationException("No se encontró el ClinicId en los Claims");

                int ClinicID = int.Parse(clinicId);

                var cliniID = await _userService.GetClinicIdByUserIdAsync(ClinicID);
               

                var results = await _resultService.GetAllPendingResultsAsync(cliniID);


                return View(results);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new List<ResultLaboratoryListViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Report(int id)
        {
            var result = await _resultService.GetResultByIdAsync(id);

            if (result is null) return NotFound();

            var resportResult = new ReportResultViewModel
            {
                ResultLaboratoryId = result.Id
            };
            return View(resportResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(ReportResultViewModel reportResultViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(reportResultViewModel);
            }

            try
            {
                await _resultService.ReportResultAsync(reportResultViewModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(reportResultViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string patientCardId)
        {
            try
            {
                var clinicIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (clinicIdClaim is null)
                    throw new InvalidOperationException("No se encontró el Clinic Id en los Claims");

                int clinicId = int.Parse(clinicIdClaim);
                int clinicIdRight = await _userService.GetClinicIdByUserIdAsync(clinicId);

                var result = await _resultService.GetPendingResultsByPatientCardIdAsync(patientCardId, clinicIdRight);
                return View("Index", result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", new List<ResultLaboratoryListViewModel>());
            }
        }

    }
}
