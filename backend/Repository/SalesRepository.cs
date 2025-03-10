using backend.Context;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class SalesRepository : IRepository
    {

        private readonly ApiDbContext _context;
        
        public SalesRepository(ApiDbContext context) {
            _context = context;
        }

        public async Task Reset() {
            _context.Database.ExecuteSqlRaw("DELETE FROM Sales");
        }

        public async Task<List<Sale>> Find(string desc, string cliente, int page, int limit)
        {
            var products = await _context.Sales
                .Include(v => v.Product)
                .Include(v => v.Customer)
                .Where(e => EF.Functions.Like(e.Product.Description, "%" + desc + "%") && EF.Functions.Like(e.Customer.Name, "%" + cliente + "%"))
                .Skip((page - 1) * limit)
                .Take(limit)
                .OrderBy(c => c.Id)
                .ToListAsync();
            return products;
        }

        public async Task<Sale> New(Sale sale, bool forceIdColumn)
        {

            if (forceIdColumn)
            {
                string query = @"
                    SET IDENTITY_INSERT [dbo].[Products] ON;
                    INSERT INTO Sales (Id, CustomerId, ProductId, Quantity, UnitaryValue, Date, Total) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7);
                    SET IDENTITY_INSERT [dbo].[Products] OFF;
                ";

                await _context.Database.ExecuteSqlRawAsync(query, sale.Id, sale.CustomerId, sale.ProductId, sale.Quantity, sale.UnitaryValue, sale.Date, sale.Total);
                return sale;
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return sale;
        }

        public async Task<Sale> Update(Sale sale) {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task Delete(int id)
        {
            var sale = await _context.Sales.FindAsync((long)id);
            if (sale == null)
            {
                throw new Exception("Venda não encontrada: " + id);
            }
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
    }
}
