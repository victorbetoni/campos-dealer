using backend.Model;
using backend.Repository;
using static backend.Usecase.Customers.CreateCustomerUsecase;

namespace backend.Usecase.Customers
{
    public class ListCustomerUsecase : UsecaseBase<CustomerRepository, ListCustomerUsecase.Input, List<Customer>> {
        public ListCustomerUsecase(ILogger<object> logger, CustomerRepository repository, Input input) : base(logger, repository, input) {
        }

        private static readonly int DEFAULT_PAGE_SIZE = 10;

        public class Input {
            public string NameFilter { get; set; }
            public int Page { get; set; }
        }

        public override OpResponse<List<Customer>> Run() {
            try {
                var clientes = _repository.FindByName(_input.NameFilter, _input.Page, DEFAULT_PAGE_SIZE);
                return new OpResponse<List<Customer>> {
                    Status = 200,
                    Message = _input.Page + "",
                    Data = clientes
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao buscar clientes");
                return new OpResponse<List<Customer>> {
                    Status = 500,
                    Message = "Algo deu errado! Tente novamente.",
                };
            }
        }
    }
}
