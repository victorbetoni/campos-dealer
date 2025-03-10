using backend.Context;
using backend.Model;
using backend.Repository;

namespace backend.Usecase.Products {
    public class ResetCustomersTablesUsecase : UsecaseBase<object, object> {

        public ResetCustomersTablesUsecase(ILogger<object> logger, ApiDbContext ctx, object input) : base(logger, ctx, input) {
        }

        public override async Task<OpResponse<object>> Run() {
            try {
                await new CustomerRepository(_context).Reset();
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
