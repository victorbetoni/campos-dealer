using backend.Context;
using backend.Model;
using backend.Repository;
using Microsoft.AspNetCore.Components.Forms;

namespace backend.Usecase.Sales {
    public class UpdateSaleUsecase : UsecaseBase<Sale, Sale> {
        public UpdateSaleUsecase(ILogger<object> logger, ApiDbContext ctx, Sale input) : base(logger, ctx, input) {
        }

        public override async Task<OpResponse<Sale>> Run() {
            try {

                var custRepo = new CustomerRepository(_context);
                var productRepo = new ProductRepository(_context);
                var saleRepo = new SalesRepository(_context);

                if ((await custRepo.FindById(_input.CustomerId)) == null) {
                    return new OpResponse<Sale> {
                        Status = 404,
                        Message = "Cliente não encontrado"
                    };
                }

                var product = await productRepo.FindById(_input.ProductId);
                if (product == null) {
                    return new OpResponse<Sale> {
                        Status = 404,
                        Message = "Produto não encontrado"
                    };
                }

                var total = product.UnitaryPrice * _input.Quantity;
                _input.UnitaryValue = product.UnitaryPrice;
                _input.Total = total;

                var existingSale = await saleRepo.FindById(_input.Id);
                if (existingSale != null) {
                    _context.Entry(existingSale).CurrentValues.SetValues(_input);
                } else {
                    await saleRepo.Update(_input);
                }

                await _context.SaveChangesAsync();

                return new OpResponse<Sale> {
                    Status = 200,
                    Message = "Produto atualizado com sucesso!",
                    Data = _input
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao atualizar venda");
                return Utils.Responses.DefaultInternalServerError<Sale>();
            }
        }


    }
}