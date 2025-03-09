using backend.Context;
using backend.Model;
using backend.Repository;
using backend.Usecase.Customers;
using backend.Usecase.Products;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers {

    [ApiController]
    [Route("/produto")]
    public class ProductController : ControllerBase {

        private readonly ProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, ApiDbContext context) {
            _logger = logger;
            this._productRepository = new ProductRepository(context);
        }

        [HttpPost]
        public OpResponse<Product> Post([FromBody] CreateProductUsecase.Input input) {
            // Poderia utilizar um middleware para fazer essa validação
            if (!ModelState.IsValid) {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return Utils.Responses.DefaultFillAllFields<Product>(errors);
            }

            if (!Utils.AllFilled(input.Description)) {
                return Utils.Responses.DefaultFillAllFields<Product>();
            }

            var res = new CreateProductUsecase(_logger, _productRepository, input).Run();
            return res;
        }

        [HttpGet]
        public OpResponse<List<Product>> Get() {
            var descFilter = Request.Query["desc"];

            var page = Utils.QueryOrDefaultInt(HttpContext, "page", 1);

            var input = new ListProductUsecase.Input {
                DescFilter = descFilter,
                Page = page
            };

            var res = new ListProductUsecase(_logger, _productRepository, input).Run();
            return res;
        }

        [HttpDelete]
        public OpResponse<object> Delete([FromBody] DeleteProductUsecase.Input id) {
            var res = new DeleteProductUsecase(_logger, _productRepository, id).Run();
            return res;
        }

        [HttpPut]
        public OpResponse<Product> Put([FromBody] Product product) {
            if (!ModelState.IsValid) {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return Utils.Responses.DefaultFillAllFields<Product>(errors);
            }

            if (!Utils.AllFilled(product.Description)) {
                return Utils.Responses.DefaultFillAllFields<Product>();
            }

            var res = new UpdateProductUsecase(_logger, _productRepository, product).Run();
            return res;
        }
    }
}
