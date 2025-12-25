using System.Threading.Tasks;
using Furni.Contexts;
using Furni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]
public class BlogController : Controller
{
    readonly FurniDbContext _context;

    public BlogController(FurniDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var blogs = await _context.Blogs.Include(x=>x.EmployeeId).ToListAsync();
        return View(blogs);
    }

    public async Task<IActionResult> Create()
    {
        var blogs = await _context.Blogs.ToListAsync();
        ViewBag.Blogs = blogs;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Blog blog)
    {
        if (!ModelState.IsValid)
        {
            var blogs = await _context.Blogs.ToListAsync();
            ViewBag.Blogs = blogs;
            return View();
        }
        var existedEmployee = await _context.Employees.AnyAsync(x => x.Id == blog.EmployeeId);
        if (!existedEmployee)
        {
            ModelState.AddModelError("EmployeeId", "Bele bir employee yoxdur");
            return View(blog);
        }

        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null) return NotFound();

       _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if(blog == null) return NotFound();
        return View(blog);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Employee employee)
    {
        var existEmployee = await _context.Employees.FirstOrDefaultAsync(_context => _context.Id == employee.Id);
        if (existEmployee == null) return NotFound();

        existEmployee.FirstName = employee.FirstName;
        existEmployee.LastName = employee.LastName;
        existEmployee.Description = employee.Description;
        existEmployee.Position = employee.Position;
        existEmployee.ImageName = employee.ImageName;
        existEmployee.ImageUrl = employee.ImageUrl;

        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
