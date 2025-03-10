using backend.Context;
using backend.Model;
using backend.Repository;
using System.Transactions;

namespace backend.Usecase.Products {
    public class BulkCreateCustomersUsecase : UsecaseBase<List<Customer>, object> {
        public BulkCreateCustomersUsecase(ILogger<object> logger, ApiDbContext ctx, List<Customer> input) : base(logger, ctx, input) {
        }

        public override async Task<OpResponse<object>> Run() {
                try {
                    var repo = new CustomerRepository(_context);
                    foreach (var p in _input) {
                        await repo.New(p, true);
                    }
                    return new OpResponse<object> {
                        Status = 200,
                        Message = "Clientes criados com sucesso!",
                        Data = null
                    };  
                } catch (Exception ex) {
                    _logger.LogError(ex, "Erro criando clientes bulk");
                    return Utils.Responses.DefaultInternalServerError<object>();
                }
        }
    }
}
