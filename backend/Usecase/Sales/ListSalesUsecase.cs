using backend.Context;
using backend.Model;
using backend.Repository;
using backend.Usecase.Customers;

namespace backend.Usecase.Sales {
    public class ListSalesUsecase : UsecaseBase<ListSalesUsecase.Input, List<Sale>> {
        public ListSalesUsecase(ILogger<object> logger, ApiDbContext ctx, Input input) : base(logger, ctx, input)
        {
        }

        private static readonly int DEFAULT_PAGE_SIZE = 10;

        public class Input {
            public string DescFilter { get; set; }
            public string NameFilter { get; set; }
            public int Page { get; set; }
        }

        public override async Task<OpResponse<List<Sale>>> Run() {
            try {
                var sales = await new SalesRepository(_context).Find(_input.DescFilter, _input.NameFilter, _input.Page, DEFAULT_PAGE_SIZE);
                return new OpResponse<List<Sale>> {
                    Status = 200,
                    Message = _input.Page + "",
                    Data = sales
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao buscar vendas");
                return Utils.Responses.DefaultInternalServerError<List<Sale>>();
            }
        }
    }
}
