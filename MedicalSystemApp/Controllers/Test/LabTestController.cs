using MedicalSystem.Application.Interfaces.Services.Test;
using MedicalSystem.Application.Interfaces.Services.UserS;
using MedicalSystem.Application.ViewModel.LabTestS;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicalSystemApp.Controllers.Test
{
    public class LabTestController : Controller
    {
        private readonly ILabTestService _labTestService;
        private readonly IUserService _userService;

        public LabTestController(ILabTestService labTestService, IUserService userService)
        {
            _labTestService = labTestService;
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

            var labTests = await _labTestService.GetLabTestsByClinicAsync(clinicId);

            return View(labTests);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new LabTestViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LabTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var clinicId = await _userService.GetClinicIdByUserIdAsync(int.Parse(userId));
                model.ClinicId = clinicId;

                bool labTestCreated = await _labTestService.CreateLabTestAsync(model);
                if (!labTestCreated)
                {
                    ModelState.AddModelError(string.Empty, "El test de laboratorio ya existe.");
                    return View(model);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var labTest = await _labTestService.GetLabTestAsync(id);
            if (labTest == null) return NotFound();

            var model = new LabTestViewModel
            {
                Id = labTest.Id,
                Name = labTest.Name,
                ClinicId = labTest.ClinicId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LabTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool labTestUpdated = await _labTestService.UpdateLabTestAsync(model);
                if (!labTestUpdated)
                {
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar el test de laboratorio.");
                    return View(model);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _labTestService.DeleteLabTestAsync(id);
            if (!result) return NotFound();

            return RedirectToAction("Index");
        }
    }
}
