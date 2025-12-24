using Microsoft.AspNetCore.Mvc;

namespace Furni.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
