using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApp.Controllers.Home
{
    public class AsitsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
