using System.Threading.Tasks;
using Furni.Contexts;
using Furni.Models;
using Microsoft.AspNetCore.Mvc;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]
public class EmployeeController(FurniDbContext context) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (!ModelState.IsValid)
            return View(employee);


        await context.Employees.AddAsync(employee);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
