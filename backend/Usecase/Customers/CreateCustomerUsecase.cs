using backend.Context;
using backend.Model;
using backend.Repository;

namespace backend.Usecase.Customers {
    public class CreateCustomerUsecase: UsecaseBase<CreateCustomerUsecase.Input, Customer> {
        public CreateCustomerUsecase(ILogger<object> logger, ApiDbContext ctx, Input input) : base(logger, ctx, input) {
        }

        public class Input {
            public string Name { get; set; }
            public string County { get; set; }
        }

        public override async Task<OpResponse<Customer>> Run() {

            var cliente = new Customer {
                Name = _input.Name.Trim(),
                County = _input.County.Trim()
            };

            try {
                await new CustomerRepository(_context).New(cliente, false);
                return new OpResponse<Customer> {
                    Status = 200,
                    Message = "Cliente criado com sucesso!",
                    Data = cliente
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao criar cliente");
                return Utils.Responses.DefaultInternalServerError<Customer>();
            }

        }
    }
}
