using backend.Model;
using backend.Repository;

namespace backend.Usecase.Customers {
    public class DeleteCustomerUsecase : UsecaseBase<CustomerRepository, DeleteCustomerUsecase.Input, object>{
        public DeleteCustomerUsecase(ILogger<object> logger, CustomerRepository repository, DeleteCustomerUsecase.Input input) : base(logger, repository, input) {
        }

        public class Input {
            public int id { get; set; }
        }

        public override OpResponse<object> Run() {
            try {
                _repository.Delete(_input.id);
                return new OpResponse<object> {
                    Status = 200,
                    Message = "Cliente deletado com sucesso!"
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao deletar cliente");
                return new OpResponse<object> {
                    Status = 500,
                    Message = "Algo deu errado! Tente novamente.",
                };
            }
        }
    }
}
