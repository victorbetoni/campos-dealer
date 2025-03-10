using backend.Context;
using backend.Model;
using backend.Repository;
using System.Transactions;

namespace backend.Usecase.Products {
    public class BulkCreateSalesUsecase : UsecaseBase<List<Sale>, object> {
        public BulkCreateSalesUsecase(ILogger<object> logger, ApiDbContext ctx, List<Sale> input) : base(logger, ctx, input) {
        }

        public override async Task<OpResponse<object>> Run() {
                try {
                    var repo = new SalesRepository(_context);
                    foreach (var p in _input) {
                        await repo.New(p, true);
                    }
                    return new OpResponse<object> {
                        Status = 200,
                        Message = "Vendas criadas com sucesso!",
                        Data = null
                    };  
                } catch (Exception ex) {
                    _logger.LogError(ex, "Erro criando vendas bulk");
                    return Utils.Responses.DefaultInternalServerError<object>();
                }
        }
    }
}
