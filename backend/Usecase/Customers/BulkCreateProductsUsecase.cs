using backend.Model;
using backend.Repository;
using System.Transactions;

namespace backend.Usecase.Products {
    public class BulkCreateCustomersUsecase : UsecaseBase<CustomerRepository, List<Customer>, object> {
        public BulkCreateCustomersUsecase(ILogger<object> logger, CustomerRepository repository, List<Customer> input) : base(logger, repository, input) {
        }

        public override async Task<OpResponse<object>> Run() {
                try {
                    foreach (var p in _input) {
                        await _repository.New(p, true);
                    }
                    return new OpResponse<object> {
                        Status = 200,
                        Message = "Clientes criados com sucesso!",
                        Data = null
                    };  
                } catch (Exception ex) {
                    _logger.LogError(ex, "Erro criando clientes bulk");
                    return Utils.Responses.DefaultInternalServerError<object>();
                }
        }
    }
}
