using System.Threading.Tasks;
using Furni.Contexts;
using Furni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController(FurniDbContext context) : Controller
{
    public IActionResult Index()
    {
        var products = context.Products.ToList();
        return View(products);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }



    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
            return View(product);

        product.CreatedDate = DateTime.UtcNow.AddHours(4);

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
}
