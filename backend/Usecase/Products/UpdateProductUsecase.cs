using backend.Context;
using backend.Model;
using backend.Repository;
namespace backend.Usecase.Products {
    public class UpdateProductUsecase : UsecaseBase<Product, Product> {
        public UpdateProductUsecase(ILogger<object> logger, ApiDbContext ctx, Product input) : base(logger, ctx, input) {
        }

        public override async Task<OpResponse<Product>> Run() {
            try {

                await new ProductRepository(_context).Update(_input);
                return new OpResponse<Product> {
                    Status = 200,
                    Message = "Venda atualizada com sucesso!",
                    Data = _input
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao atualizar produto");
                return Utils.Responses.DefaultInternalServerError<Product>();
            }
        }


    }
}