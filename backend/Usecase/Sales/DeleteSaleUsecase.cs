using backend.Context;
using backend.Model;
using backend.Repository;

namespace backend.Usecase.Sales {
    public class DeleteSaleUsecase : UsecaseBase<DeleteSaleUsecase.Input, object>{
        public DeleteSaleUsecase(ILogger<object> logger, ApiDbContext ctx, DeleteSaleUsecase.Input input) : base(logger, ctx, input) {
        }

        public class Input {
            public int id { get; set; }
        }

        public override async Task<OpResponse<object>> Run() {
            try {
                await new SalesRepository(_context).Delete(_input.id);
                return new OpResponse<object> {
                    Status = 200,
                    Message = "Venda deletada com sucesso!"
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao deletar venda");
                return Utils.Responses.DefaultInternalServerError<object>();
            }
        }
    }
}
