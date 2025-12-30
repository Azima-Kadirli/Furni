using Furni.Contexts;
using Furni.Models;
using Furni.ViewModels.ServiceViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]
public class ServiceController : Controller
{
    readonly FurniDbContext _context;
    readonly IWebHostEnvironment _env;

    public ServiceController(FurniDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var services = await _context.Services.Select
            (s=>new ServiceIndexViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageName =  s.ImageName,
                ImageUrl =  s.ImageUrl
            })
            .ToListAsync();
        return View(services);
    }

    public async Task<IActionResult> Create()
    {
        var services = await _context.Services.ToListAsync();
        ViewBag.Services = services;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ServiceCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var services = await _context.Services.ToListAsync();
            ViewBag.Service = services;
            return View();
        }
        string uniqueImageName = Guid.NewGuid().ToString() + vm.Image.FileName;
        string mainPath = Path.Combine(_env.WebRootPath, "assets", "images", uniqueImageName);
        using FileStream mainStream = new(mainPath, FileMode.Create);
        await vm.Image.CopyToAsync(mainStream);



        if (!vm.Image.ContentType.Contains("image"))
        {
            ModelState.AddModelError("Image", "Yalniz image tipinde sekiller yukleyin");
            return View(vm);
        }

        if (vm.Image.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("Image", "Sekilin uzunlugu maksimum 2mb ola biler");
        }
        var service = new Service()
        {
            Name = vm.Name,
            Description = vm.Description,
            ImageName = uniqueImageName,
            ImageUrl = Path.Combine("assets", "images")
        };
        await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if(service == null) return NotFound();

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
        if (service is null) return NotFound("Service is not found");
        var vm = new ServiceUpdateViewModel()
        {
            Name = service.Name,
            Description = service.Description,
            Image = service.ImageName,
            ImageUrl = service.ImageUrl
        };
        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(ServiceUpdateViewModel vm)
    {
        var existService = await _context.Services.FirstOrDefaultAsync(x=>x.Id==vm.Id);
        if (existService == null) return NotFound();

        existService.Description = vm.Description;
        existService.Name = vm.Name;
        existService.ImageName = vm.Image;
        existService.ImageUrl = vm.ImageUrl;

        _context.Services.Update(existService);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}