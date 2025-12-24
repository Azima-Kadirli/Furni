using System.Threading.Tasks;
using Furni.Contexts;
using Furni.Models;
using Microsoft.AspNetCore.Mvc;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]

public class DashboardController():Controller
{
    public IActionResult Index()
    {
        return View();
    }

}
