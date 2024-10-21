using GrpcSubProject.Model;
using Microsoft.EntityFrameworkCore;

namespace GrpcSubProject.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
