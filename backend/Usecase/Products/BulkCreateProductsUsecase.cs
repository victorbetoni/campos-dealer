using backend.Model;
using backend.Repository;
using System.Transactions;

namespace backend.Usecase.Products {
    public class BulkCreateProductsUsecase : UsecaseBase<ProductRepository, List<Product>, object> {
        public BulkCreateProductsUsecase(ILogger<object> logger, ProductRepository repository, List<Product> input) : base(logger, repository, input) {
        }

        public override async Task<OpResponse<object>> Run() {
                try {
                    foreach (var p in _input) {
                        await _repository.New(p, true);
                    }
                    return new OpResponse<object> {
                        Status = 200,
                        Message = "Produtos criados com sucesso!",
                        Data = null
                    };  
                } catch (Exception ex) {
                    _logger.LogError(ex, "Erro criando produtos bulk");
                    return Utils.Responses.DefaultInternalServerError<object>();
                }
        }
    }
}
