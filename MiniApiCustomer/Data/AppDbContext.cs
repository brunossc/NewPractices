using Microsoft.EntityFrameworkCore;

namespace MinimalApiCustomer.Data;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}