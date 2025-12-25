using Furni.Models;
using Microsoft.EntityFrameworkCore;

namespace Furni.Contexts;

public class FurniDbContext: DbContext
{
    public FurniDbContext(DbContextOptions option) : base(option)
    {

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Blog>Blogs { get; set; }
    public DbSet<Customer> Customers { get; set; }

}
