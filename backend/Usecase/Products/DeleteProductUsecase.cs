using backend.Model;
using backend.Repository;

namespace backend.Usecase.Products {
    public class DeleteProductUsecase : UsecaseBase<ProductRepository, DeleteProductUsecase.Input, object>{
        public DeleteProductUsecase(ILogger<object> logger, ProductRepository repository, DeleteProductUsecase.Input input) : base(logger, repository, input) {
        }

        public class Input {
            public int id { get; set; }
        }

        public override async Task<OpResponse<object>> Run() {
            try {
                await _repository.Delete(_input.id);
                return new OpResponse<object> {
                    Status = 200,
                    Message = "Produto deletado com sucesso!"
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao deletar produto");
                return Utils.Responses.DefaultInternalServerError<object>();
            }
        }
    }
}
