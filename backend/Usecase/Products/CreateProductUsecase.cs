using backend.Model;
using backend.Repository;

namespace backend.Usecase.Products {
    public class CreateProductUsecase : UsecaseBase<ProductRepository, CreateProductUsecase.Input, Product> {
        public CreateProductUsecase(ILogger<object> logger, ProductRepository repository, Input input) : base(logger, repository, input) {
        }

        public class Input {
            public string Description { get; set; }
            public float UnitaryPrice { get; set; }
        }

        public override OpResponse<Product> Run() {

            var product = new Product {
                Description = _input.Description.Trim(),
                UnitaryPrice = _input.UnitaryPrice
            };

            try {
                _repository.New(product);
                return new OpResponse<Product> {
                    Status = 200,
                    Message = "Cliente criado com sucesso!",
                    Data = product
                };
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro ao criar produto");
                return Utils.Responses.DefaultInternalServerError<Product>();
            }

        }
    }
}
