using backend.Context;
using backend.Model;
using backend.Repository;

namespace backend.Usecase.Sales {
    public class CreateSaleUsecase: UsecaseBase<CreateSaleUsecase.Input, Sale> {
        public CreateSaleUsecase(ILogger<object> logger, ApiDbContext ctx, Input input) : base(logger, ctx, input) {
        }

        public class Input {

            public int ProductId { get; set; }
            public int CustomerId { get; set; }
            public int Quantity { get; set; }

        }

        public override async Task<OpResponse<Sale>> Run() {

            var custRepo = new CustomerRepository(_context);
            var productRepo = new ProductRepository(_context);
            var saleRepository = new SalesRepository(_context);

            try {
                var customer = await custRepo.FindById(_input.CustomerId);
                if (customer == null) {
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
                var date = DateTime.Now;

                var sale = new Sale {
                    Product = product,
                    Customer = customer,
                    Quantity = _input.Quantity,
                    Total = (decimal) total,
                    Date = date,
                    CustomerId = _input.CustomerId,
                    ProductId = _input.ProductId,
                    UnitaryValue = product.UnitaryPrice
                };

                await saleRepository.New(sale, false);
                return new OpResponse<Sale> {
                    Status = 200,
                    Message = "Venda criada com sucesso!",
                    Data = sale
                };

            }
            catch (Exception ex) {
                _logger.LogError(ex, "Erro ao criar venda");
                return Utils.Responses.DefaultInternalServerError<Sale>();
            }

        }
    }
}
