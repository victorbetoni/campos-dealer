using backend.Context;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository {
    public class CustomerRepository: IRepository{

        private readonly ApiDbContext _context;

        public CustomerRepository(ApiDbContext context) {
            _context = context;
        }

        public List<Customer> FindByName(string name, int page, int limit) {
            var customers = _context.Customers
                .Where(e => EF.Functions.Like(e.Name, "%" + name + "%"))
                .Skip((page - 1) * limit)
                .Take(limit)
                .OrderBy(c => c.Id)
                .ToList();
            return customers;
        }

        public Customer New(Customer cliente) {
            _context.Customers.Add(cliente);
            _context.SaveChanges();
            return cliente;
        }

        public Customer Update(Customer cliente) {
            _context.Customers.Update(cliente);
            _context.SaveChanges();
            return cliente;
        }

        public void Delete(int idCliente) {
            // deletar as vendas referentes a esse cliente antes
            var cliente = _context.Customers.Find((long) idCliente);
            if (cliente == null) {
                throw new Exception("Cliente não encontrado: " + idCliente);
            }
            _context.Customers.Remove(cliente);
            _context.SaveChanges();
        }
    }

}
