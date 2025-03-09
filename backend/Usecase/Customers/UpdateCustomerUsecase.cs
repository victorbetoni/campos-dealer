using backend.Model;
using backend.Repository;
using Microsoft.AspNetCore.Components.Forms;

namespace backend.Usecase.Customers
{
    public class UpdateCustomerUsecase : UsecaseBase<CustomerRepository, Customer, Customer>
    {
        public UpdateCustomerUsecase(ILogger<object> logger, CustomerRepository repository, Customer input) : base(logger, repository, input) {
        }

        public override OpResponse<Customer> Run() {

            var cliente = new Customer {
                Name = _input.Name.Trim(),
                County = _input.County.Trim()
            };

            try {
                _repository.Update(_input);
                return new OpResponse<Customer> {
                    Status = 200,
                    Message = "Cliente atualizado com sucesso!",
                    Data = _input
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao atualizar cliente");
                return new OpResponse<Customer> {
                    Status = 500,
                    Message = "Algo deu errado! Tente novamente.",
                };
            }
        }


    }
}