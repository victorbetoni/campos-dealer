using backend.Context;
using backend.Model;
using backend.Repository;

namespace backend.Usecase.Customers {
    public class DeleteCustomerUsecase : UsecaseBase<DeleteCustomerUsecase.Input, object>{
        public DeleteCustomerUsecase(ILogger<object> logger, ApiDbContext ctx, DeleteCustomerUsecase.Input input) : base(logger, ctx, input) {
        }

        public class Input {
            public int id { get; set; }
        }

        public override async Task<OpResponse<object>> Run() {
            try {
                await new CustomerRepository(_context).Delete(_input.id);
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
