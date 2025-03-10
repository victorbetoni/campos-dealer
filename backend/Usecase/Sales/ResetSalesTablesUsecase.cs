using backend.Context;
using backend.Model;
using backend.Repository;

namespace backend.Usecase.Products {
    public class ResetSalesTablesUsecase : UsecaseBase<object, object> {

        public ResetSalesTablesUsecase(ILogger<object> logger, ApiDbContext ctx, object input) : base(logger, ctx, input) {
        }

        public override async Task<OpResponse<object>> Run() {
            try {
                await new SalesRepository(_context).Reset();
                return new OpResponse<object> {
                    Status = 200,
                    Message = "Tabela de vendas resetadas com sucesso!"
                };
            } catch(Exception ex) {
                return Utils.Responses.DefaultInternalServerError<object>();
            }
        }
    }
}
