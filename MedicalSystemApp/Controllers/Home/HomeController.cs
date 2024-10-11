using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApp.Controllers.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
