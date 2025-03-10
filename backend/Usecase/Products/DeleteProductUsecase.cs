using backend.Context;
using backend.Model;
using backend.Repository;
using System.Transactions;

namespace backend.Usecase.Products {
    public class DeleteProductUsecase : UsecaseBase<DeleteProductUsecase.Input, object>{
        public DeleteProductUsecase(ILogger<object> logger, ApiDbContext ctx, DeleteProductUsecase.Input input) : base(logger, ctx, input) {
        }

        public class Input {
            public int id { get; set; }
        }

        public override async Task<OpResponse<object>> Run() {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
                try
                {
                    await new SalesRepository(_context).DeleteWithProduct(_input.id);
                    await new ProductRepository(_context).Delete(_input.id);
                    scope.Complete();
                    return new OpResponse<object> {
                        Status = 200,
                        Message = "Produto deletado com sucesso!"
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao deletar produto");
                    return Utils.Responses.DefaultInternalServerError<object>();
                }
            }
        }
    }
}
