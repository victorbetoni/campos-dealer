using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Context {
    public class ApiDbContext : DbContext{
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
