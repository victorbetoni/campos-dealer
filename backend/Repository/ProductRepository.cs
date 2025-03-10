using backend.Context;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository {
    public class ProductRepository : IRepository {

        private readonly ApiDbContext _context;

        public ProductRepository(ApiDbContext context) {
            _context = context;
        }

        public async Task Reset() {
            _context.Database.ExecuteSqlRaw("DELETE FROM Products");
        }

        public async Task<List<Product>> FindByDesc(string desc, int page, int limit) {
            var products = await _context.Products
                .Where(e => EF.Functions.Like(e.Description, "%" + desc + "%"))
                .Skip((page - 1) * limit)
                .Take(limit)
                .OrderBy(c => c.Id)
                .ToListAsync();
            return products;
        }

        public async Task<Product> New(Product product, bool forceIdColumn) {

            if(forceIdColumn) {
                string query = @"
                    SET IDENTITY_INSERT [dbo].[Products] ON;
                    INSERT INTO Products (Id, Description, UnitaryPrice) VALUES (@p0, @p1, @p2);
                    SET IDENTITY_INSERT [dbo].[Products] OFF;
                ";

                await _context.Database.ExecuteSqlRawAsync(query, product.Id, product.Description, product.UnitaryPrice);
                return product;
            } 

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> Update(Product product) {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Delete(int idProduct) {
            var product = await _context.Products.FindAsync((long)idProduct);
            if (product == null) {
                throw new Exception("Produto não encontrado: " + idProduct);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
