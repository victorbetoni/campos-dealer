using backend.Context;
using backend.Model;
using backend.Repository;
using backend.Usecase.Customers;

namespace backend.Usecase.Products {
    public class ListProductUsecase : UsecaseBase<ListProductUsecase.Input, List<Product>> {
        public ListProductUsecase(ILogger<object> logger, ApiDbContext ctx, Input input) : base(logger, ctx, input)
        {
        }

        private static readonly int DEFAULT_PAGE_SIZE = 10;

        public class Input {
            public string DescFilter { get; set; }
            public int Page { get; set; }
        }

        public override async Task<OpResponse<List<Product>>> Run() {
            try {
                var products = await new ProductRepository(_context).FindByDesc(_input.DescFilter, _input.Page, DEFAULT_PAGE_SIZE);
                return new OpResponse<List<Product>> {
                    Status = 200,
                    Message = _input.Page + "",
                    Data = products
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao buscar produtos");
                return Utils.Responses.DefaultInternalServerError<List<Product>>();
            }
        }
    }
}
