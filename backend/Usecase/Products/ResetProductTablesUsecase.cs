using backend.Model;
using backend.Repository;

namespace backend.Usecase.Products {
    public class ResetProductTablesUsecase : UsecaseBase<ProductRepository, object, object> {

        public ResetProductTablesUsecase(ILogger<object> logger, ProductRepository repository, object input) : base(logger, repository, input) {
        }

        public override async Task<OpResponse<object>> Run() {
            try {
                await _repository.Reset();
                return new OpResponse<object> {
                    Status = 200,
                    Message = "Tabelas de produtos resetadas com sucesso!"
                };
            } catch(Exception ex) {
                return Utils.Responses.DefaultInternalServerError<object>();
            }
        }
    }
}
