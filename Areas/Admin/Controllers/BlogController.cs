using System.Threading.Tasks;
using Azure;
using Furni.Contexts;
using Furni.Models;
using Furni.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]
public class BlogController : Controller
{
    readonly FurniDbContext _context;
    readonly IWebHostEnvironment _webHostEnvironment;

    public BlogController(FurniDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var blogs = await _context.Blogs.Include(x=>x.Employee).Select(b=>new BlogGetViewModel
        {
            Id = b.Id,
            PostDate = b.PostDate,
            ImageName = b.ImageName,
            Text = b.Text,
            Title = b.Title,
            TagNames=b.BlogTags.Select(bt=>bt.Tag.Name).ToList(),
            Employee = b.Employee
        }).ToListAsync();
        return View(blogs);
    }

    public async Task<IActionResult> Create()
    {

        ViewBag.Employees = await _context.Employees.ToListAsync();
        ViewBag.Tags = await _context.Tags.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateViewModel vm)
    {

        ViewBag.Employees = await _context.Employees.ToListAsync();
        ViewBag.Tags = await _context.Tags.ToListAsync();
        if (vm.ImageName == null)
        {
            ModelState.AddModelError("ImageName", "Şəkil seçilməyib");
        }
        else
        {
            if (!vm.ImageName.ContentType.Contains("image"))
                ModelState.AddModelError("ImageName", "Yalnız image tipində şəkil yükləyin");

            if (vm.ImageName.Length > 2 * 1024 * 1024)
                ModelState.AddModelError("ImageName", "Şəkilin ölçüsü maksimum 2MB ola bilər");
        }

        if (vm.EmployeeId == 0 || !await _context.Employees.AnyAsync(e => e.Id == vm.EmployeeId))
        {
            ModelState.AddModelError("EmployeeId", "Employee seçilməyib və ya düzgün deyil");
        }

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        string uniqueFileName = Guid.NewGuid() + vm.ImageName.FileName;
        string path = Path.Combine(_webHostEnvironment.WebRootPath,"assets", "images", uniqueFileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await vm.ImageName.CopyToAsync(stream);
        }

        Blog blog = new()
        {
            Title = vm.Title,
            Text = vm.Text,
            PostDate = DateTime.UtcNow.AddHours(4),
            ImageUrl = "/assets/images/" + uniqueFileName,
            EmployeeId = vm.EmployeeId,
            ImageName = uniqueFileName,
            BlogTags = []
        };
        foreach (var tagId in vm.TagIds)
        {
            BlogTag blogTag = new()
            {
                TagId = tagId,
                Blog = blog
            };
            blog.BlogTags.Add(blogTag);
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
    public async Task<IActionResult> Update(BlogUpdateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var existBlog = await _context.Blogs.FindAsync(vm.Id);
        if (existBlog == null) return NotFound();
        existBlog.Title = vm.Title;
        existBlog.Text = vm.Text;

        _context.Blogs.Update(existBlog);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Detail(int id)
    {
        var blog = await _context.Blogs
            .Select(product => new BlogGetViewModel()
            {
                Id = product.Id,
                ImageName = product.ImageName,
                Text = product.Text,
                Title = product.Title,
                PostDate = product.PostDate,
                TagNames = product.BlogTags.Select(x => x.Tag.Name).ToList()
            }).FirstOrDefaultAsync(x => x.Id == id);

        if (blog is null)
            return NotFound();
        return View(blog);
    }
}
