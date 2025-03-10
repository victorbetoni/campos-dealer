using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Context {
    public class ApiDbContext : DbContext{
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }
}
