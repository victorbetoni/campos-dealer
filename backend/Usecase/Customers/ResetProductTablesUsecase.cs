using backend.Model;
using backend.Repository;

namespace backend.Usecase.Products {
    public class ResetCustomersTablesUsecase : UsecaseBase<CustomerRepository, object, object> {

        public ResetCustomersTablesUsecase(ILogger<object> logger, CustomerRepository repository, object input) : base(logger, repository, input) {
        }

        public override async Task<OpResponse<object>> Run() {
            try {
                await _repository.Reset();
                return new OpResponse<object> {
                    Status = 200,
                    Message = "Tabelas de customers resetadas com sucesso!"
                };
            } catch(Exception ex) {
                return Utils.Responses.DefaultInternalServerError<object>();
            }
        }
    }
}
