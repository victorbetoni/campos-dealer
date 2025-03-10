using backend.Context;
using backend.Model;
using backend.Repository;
using System.Transactions;

namespace backend.Usecase.Customers {
    public class DeleteCustomerUsecase : UsecaseBase<DeleteCustomerUsecase.Input, object>{
        public DeleteCustomerUsecase(ILogger<object> logger, ApiDbContext ctx, DeleteCustomerUsecase.Input input) : base(logger, ctx, input) {
        }

        public class Input {
            public int id { get; set; }
        }

        public override async Task<OpResponse<object>> Run() {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
                try {
                    await new SalesRepository(_context).DeleteWithCustomer(_input.id);
                    await new CustomerRepository(_context).Delete(_input.id);
                    scope.Complete();
                    return new OpResponse<object> {
                        Status = 200,
                        Message = "Cliente deletado com sucesso!"
                    };
                } catch (Exception ex) {
                    _logger.LogError(ex, "Erro ao deletar cliente");
                    return Utils.Responses.DefaultInternalServerError<object>();
                }
            }
        }
    }
}
