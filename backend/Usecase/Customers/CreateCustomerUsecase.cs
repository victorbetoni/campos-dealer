using backend.Model;
using backend.Repository;

namespace backend.Usecase.Customers {
    public class CreateCustomerUsecase: UsecaseBase<CustomerRepository, CreateCustomerUsecase.Input, Customer> {
        public CreateCustomerUsecase(ILogger<object> logger, CustomerRepository repository, Input input) : base(logger, repository, input) {
        }

        public class Input {
            public string Name { get; set; }
            public string County { get; set; }
        }

        public override OpResponse<Customer> Run() {

            var cliente = new Customer {
                Name = _input.Name.Trim(),
                County = _input.County.Trim()
            };

            if (string.IsNullOrEmpty(cliente.Name) || string.IsNullOrEmpty(cliente.County)) {
                return new OpResponse<Customer> {
                    Status = 400,
                    Message = "Preencha todos os campos corretamente."
                };
            }

            try {
                _repository.New(cliente);
                return new OpResponse<Customer> {
                    Status = 200,
                    Message = "Cliente criado com sucesso!",
                    Data = cliente
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao criar cliente");
                return new OpResponse<Customer> {
                    Status = 500,
                    Message = "Algo deu errado! Tente novamente.",
                };
            }

        }
    }
}
