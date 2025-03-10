using backend.Context;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository {
    public class CustomerRepository : IRepository
    {

        private readonly ApiDbContext _context;

        public CustomerRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task Reset() {
            _context.Database.ExecuteSqlRaw("DELETE FROM Customers");
        }

        public async Task<Customer> FindById(long id) {
            var cliente = await _context.Customers.FindAsync((long)id);
            return cliente;
        }

        public async Task<List<Customer>> FindByName(string name, int page, int limit) {
            if (page < 0) {
                return await _context.Customers
                    .Where(e => EF.Functions.Like(e.Name, "%" + name + "%"))
                    .OrderBy(c => c.Id)
                    .ToListAsync();
            }
            return await _context.Customers
                .Where(e => EF.Functions.Like(e.Name, "%" + name + "%"))
                .Skip((page - 1) * limit)
                .Take(limit)
                .OrderBy(c => c.Id)
                .ToListAsync();
        }

        public async Task<Customer> New(Customer cliente, bool forceIdColumn) {
            if (forceIdColumn)
            {
                string query = @"
                    SET IDENTITY_INSERT [dbo].[Customers] ON;
                    INSERT INTO Customers (Id, Name, County) VALUES (@p0, @p1, @p2);
                    SET IDENTITY_INSERT [dbo].[Customers] OFF;
                ";

                await _context.Database.ExecuteSqlRawAsync(query, cliente.Id, cliente.Name, cliente.County);
                return cliente;
            }

            _context.Customers.Add(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task<Customer> Update(Customer cliente) {
            _context.Customers.Update(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task Delete(int idCliente) { 
            // deletar as vendas referentes a esse cliente antes
            var cliente = await _context.Customers.FindAsync((long)idCliente);
            if (cliente == null) {
                throw new Exception("Cliente não encontrado: " + idCliente);
            }
            _context.Customers.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }

}
