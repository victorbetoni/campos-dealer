using backend.Context;
using backend.Model;
using backend.Repository;
using backend.Usecase.Products;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Transactions;

namespace backend.Controllers {

    [ApiController]
    [Route("/dataLoad")]
    public class DataLoadController : ControllerBase {

        public class CustomerInput {
            public int idCliente;
            public string nmCliente;
            public string Cidade;
        }

        public class ProductInput {
            public int idProduto;
            public string dscProduto;
            public int vlrUnitario;
        }

        public class SalesInput {

        }

        private readonly ILogger<DataLoadController> _logger;
        private readonly ApiDbContext _context;

        public DataLoadController(ILogger<DataLoadController> logger, ApiDbContext context) {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<OpResponse<object>> Load() {

            var productsUrl = "https://camposdealer.dev/Sites/TesteAPI/produto";
            var custsUrl = "https://camposdealer.dev/Sites/TesteAPI/cliente";

            using (var client = new HttpClient()) 
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {

                try
                {
                    if (!(await new ResetProductTablesUsecase(_logger, _context, new object { }).Run()).Ok())
                    {
                        return Utils.Responses.DefaultInternalServerError<object>();
                    }

                    if (!(await new ResetCustomersTablesUsecase(_logger, _context, new object { }).Run()).Ok())
                    {
                        return Utils.Responses.DefaultInternalServerError<object>();
                    }

                    if (!(await new ResetProductTablesUsecase(_logger, _context, new object { }).Run()).Ok())
                    {
                        return Utils.Responses.DefaultInternalServerError<object>();
                    }


                    var prodRes = await client.GetStringAsync(productsUrl);
                    var products = JsonConvert.DeserializeObject<List<ProductInput>>(prodRes.Trim('"').Replace("\\", ""));
                    var prodMapped = products.Select(pr => new Product { Id = pr.idProduto, Description = pr.dscProduto, UnitaryPrice = pr.vlrUnitario }).ToList();
                    var prodResult = await new BulkCreateProductsUsecase(_logger, _context, prodMapped).Run();
                    if (!prodResult.Ok())
                    {
                        return prodResult;
                    }

                    var custsRes = await client.GetStringAsync(custsUrl);
                    var customers = JsonConvert.DeserializeObject<List<CustomerInput>>(custsRes.Trim('"').Replace("\\", ""));
                    var custMapped = customers.Select(pr => new Customer { Id = pr.idCliente, Name = pr.nmCliente, County = pr.Cidade }).ToList();
                    var custResult = await new BulkCreateCustomersUsecase(_logger, _context, custMapped).Run();
                    if (!custResult.Ok())
                    {
                        return custResult;
                    }

                    transactionScope.Complete();
                    return new OpResponse<object>
                    {
                        Status = 200,
                        Message = "Dados importados com sucesso!"
                    };
                } catch(Exception ex)
                {
                    _logger.LogError(ex, "Erro durante data loading");
                    return Utils.Responses.DefaultInternalServerError<object>();
                }
            }
        }

        [HttpPost("/produtos")]
        public OpResponse<object> LoadProducts() {
            return new OpResponse<object> {
                Status = 200,
                Message = "Data loaded successfully",
            };
        }

        [HttpPost("/vendas")]
        public OpResponse<object> LoadSales() {
            return new OpResponse<object> {
                Status = 200,
                Message = "Data loaded successfully"
            };
        }

    }
}
