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

        
        string folder = Path.Combine("assets", "images");
        string uniqueName = Guid.NewGuid().ToString()+Path.GetExtension(vm.Image.FileName);
        string imagePath = Path.Combine(env.WebRootPath, folder,uniqueName);
        using FileStream stream = new(imagePath, FileMode.Create);
        await vm.Image.CopyToAsync(stream);

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
            ImageName = uniqueName,
            ImagePath = Path.Combine(folder,uniqueName)
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
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Product product)
    {
        if(!ModelState.IsValid) return View(product);
        var existingProduct = await context.Products.FirstOrDefaultAsync(p=>p.Id == product.Id);
        if (existingProduct is null) return NotFound("Product is not found");
        existingProduct.Title = product.Title;
        existingProduct.Price = product.Price;
        existingProduct.CreatedDate = DateTime.UtcNow.AddHours(4);
        existingProduct.IsDeleted = product.IsDeleted;
        existingProduct.ImageName = product.ImageName;
        existingProduct.ImagePath = product.ImagePath;
        existingProduct.UpdatedDate = product.UpdatedDate;

        context.Products.Update(existingProduct);
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
