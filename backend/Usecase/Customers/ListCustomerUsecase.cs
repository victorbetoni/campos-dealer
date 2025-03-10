using backend.Context;
using backend.Model;
using backend.Repository;

namespace backend.Usecase.Customers
{
    public class ListCustomerUsecase : UsecaseBase<ListCustomerUsecase.Input, List<Customer>> {
        public ListCustomerUsecase(ILogger<object> logger, ApiDbContext ctx, Input input) : base(logger, ctx, input) {
        }

        private static readonly int DEFAULT_PAGE_SIZE = 10;

        public class Input {
            public string NameFilter { get; set; }
            public int Page { get; set; }
        }

        public override async Task<OpResponse<List<Customer>>> Run() {
            try {
                var clientes = await new CustomerRepository(_context).FindByName(_input.NameFilter, _input.Page, DEFAULT_PAGE_SIZE);
                return new OpResponse<List<Customer>> {
                    Status = 200,
                    Message = _input.Page + "",
                    Data = clientes
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao buscar clientes");
                return Utils.Responses.DefaultInternalServerError<List<Customer>>();
            }
        }
    }
}
