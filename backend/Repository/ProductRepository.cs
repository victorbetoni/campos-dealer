using backend.Context;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository {
    public class ProductRepository : IRepository {

        private readonly ApiDbContext _context;

        public ProductRepository(ApiDbContext context) {
            _context = context;
        }

        public List<Product> FindByDesc(string desc, int page, int limit) {
            var products = _context.Products
                .Where(e => EF.Functions.Like(e.Description, "%" + desc + "%"))
                .Skip((page - 1) * limit)
                .Take(limit)
                .OrderBy(c => c.Id)
                .ToList();
            return products;
        }

        public Product New(Product product) {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product Update(Product product) {
            _context.Products.Update(product);
            _context.SaveChanges();
            return product;
        }

        public Product Delete(int idProduct) {
            var product = _context.Products.Find((long)idProduct);
            if (product == null) {
                throw new Exception("Produto não encontrado: " + idProduct);
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return product;
        }
    }
}
