using backend.Model;
using backend.Repository;
using Microsoft.AspNetCore.Components.Forms;

namespace backend.Usecase.Products {
    public class UpdateProductUsecase : UsecaseBase<ProductRepository, Product, Product> {
        public UpdateProductUsecase(ILogger<object> logger, ProductRepository repository, Product input) : base(logger, repository, input) {
        }

        public override OpResponse<Product> Run() {
            try {
                _repository.Update(_input);
                return new OpResponse<Product> {
                    Status = 200,
                    Message = "Produto atualizado com sucesso!",
                    Data = _input
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao atualizar produto");
                return Utils.Responses.DefaultInternalServerError<Product>();
            }
        }


    }
}