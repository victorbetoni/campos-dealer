using backend.Context;
using backend.Model;
using backend.Repository;
using Microsoft.AspNetCore.Components.Forms;

namespace backend.Usecase.Customers
{
    public class UpdateCustomerUsecase : UsecaseBase<Customer, Customer>
    {
        public UpdateCustomerUsecase(ILogger<object> logger, ApiDbContext ctx, Customer input) : base(logger, ctx, input) {
        }

        public override async Task<OpResponse<Customer>> Run() {

            var cliente = new Customer {
                Name = _input.Name.Trim(),
                County = _input.County.Trim()
            };

            try {
                await new CustomerRepository(_context).Update(_input);
                return new OpResponse<Customer> {
                    Status = 200,
                    Message = "Cliente atualizado com sucesso!",
                    Data = _input
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao atualizar cliente");
                return Utils.Responses.DefaultInternalServerError<Customer>();
            }
        }


    }
}