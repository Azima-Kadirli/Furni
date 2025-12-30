using System.Threading.Tasks;
using Furni.Contexts;
using Furni.Models;
using Furni.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController(FurniDbContext context,IWebHostEnvironment env) : Controller
{
    public async Task<IActionResult> Index()
    {
        var products = await context.Products
            .Select(p=>new ProductIndexViewModel
            {
                Id = p.Id,
                ImageName = p.ImageName,
                ImagePath = p.ImagePath,
                IsDeleted = p.IsDeleted,
                Title = p.Title,
                Price = p.Price
            }).ToListAsync();
        return View(products);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        //product.CreatedDate = DateTime.UtcNow.AddHours(4);
        string uniqueImageName = Guid.NewGuid().ToString() + vm.Image.FileName;
        string mainPath = Path.Combine(env.WebRootPath, "assets", "images", uniqueImageName);
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

        Product product = new()
        {
            Title = vm.Title,
            Price = vm.Price,
            CreatedDate = DateTime.UtcNow.AddHours(4),
            IsDeleted = false,
            ImageName = uniqueImageName,
            ImagePath = Path.Combine("assets", "images")
        };

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var product = await context.Products.FindAsync(id);
        if(product is null) 
            return View();
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update([FromRoute]int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product is null) return NotFound("Product is not found");
        var vm = new ProductUpdateViewModel()
        {
            Title =  product.Title,
            Price = product.Price,
            ImageName = product.ImageName,
            ImagePath = product.ImagePath
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var products = await context.Products.ToListAsync();
            ViewBag.Products = products;
            return View(vm);
        }
        var product = await context.Products.FindAsync(vm.Id);
        if (product is null)
            return NotFound();
        product.Title = vm.Title;
        product.Price = vm.Price;
        product.ImageName = vm.ImageName;
        product.UpdatedDate = vm.UpdatedDate;

        context.Products.Update(product);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Toggle(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is not { })
        {
            return NotFound("Product is not found");
        }
        product.IsDeleted = !product.IsDeleted;
        context.Update(product);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
