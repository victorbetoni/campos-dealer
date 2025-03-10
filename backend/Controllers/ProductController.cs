using backend.Context;
using backend.Model;
using backend.Repository;
using backend.Usecase.Customers;
using backend.Usecase.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace backend.Controllers {

    [ApiController]
    [Route("/produto")]
    public class ProductController : ControllerBase {

        private readonly ApiDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, ApiDbContext context) {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<OpResponse<Product>> Post([FromBody] CreateProductUsecase.Input input) {
            // Poderia utilizar um middleware para fazer essa validação
            if (!ModelState.IsValid) {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return Utils.Responses.DefaultFillAllFields<Product>(errors);
            }

            if (!Utils.AllFilled(input.Description)) {
                return Utils.Responses.DefaultFillAllFields<Product>();
            }

            if (input.UnitaryPrice <= 0 || input.UnitaryPrice > 3.40e+38f) {
                return new OpResponse<Product> {
                    Status = 400,
                    Message = "Preço inválido."
                };
            }

            return await new CreateProductUsecase(_logger, _context, input).Run();
        }

        [HttpGet]
        public async Task<OpResponse<List<Product>>> Get() {
            var descFilter = Request.Query["desc"];

            var page = Utils.QueryOrDefaultInt(HttpContext, "page", 1);

            var input = new ListProductUsecase.Input {
                DescFilter = descFilter,
                Page = page
            };

            return await new ListProductUsecase(_logger, _context, input).Run();
        }

        [HttpDelete]
        public async Task<OpResponse<object>> Delete([FromBody] DeleteProductUsecase.Input id) {
            return await new DeleteProductUsecase(_logger, _context, id).Run();
        }

        [HttpPut]
        public async Task<OpResponse<Product>> Put([FromBody] Product product) {
            if (!ModelState.IsValid) {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return Utils.Responses.DefaultFillAllFields<Product>(errors);
            }

            if (!Utils.AllFilled(product.Description)) {
                return Utils.Responses.DefaultFillAllFields<Product>();
            }

            return await new UpdateProductUsecase(_logger, _context, product).Run();
        }
    }
}
