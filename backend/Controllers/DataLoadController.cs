using backend.Context;
using backend.Model;
using backend.Repository;
using backend.Usecase.Products;
using Microsoft.AspNetCore.Http;
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
            public int idVenda;
            public string dthVenda;
            public int idCliente;
            public int idProduto;
            public int qtdVenda;
            public int vlrUnitarioVenda;
        }

        private readonly ILogger<DataLoadController> _logger;
        private readonly ApiDbContext _context;

        public DataLoadController(ILogger<DataLoadController> logger, ApiDbContext context) {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<OpResponse<object>> Load() {

            /*
             * Como não estava no escopo do teste técnico, optei por não
             * implementar autenticação. Em uma operação tão crítica como 
             * essa que faz DROP das tabelas, é imprescindível utilizar
             * uma autenticação e verificar se o usuário tem permissão
             * para tal.
             */
            var productsUrl = "https://camposdealer.dev/Sites/TesteAPI/produto";
            var custsUrl = "https://camposdealer.dev/Sites/TesteAPI/cliente";
            var salesUrl = "https://camposdealer.dev/Sites/TesteAPI/venda";

            using (var client = new HttpClient()) 
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {

                try
                {

                    // DROP de todas as tabelas antes de carregar dados, evitando violações de PK.
                    // Realizando na ordem Product -> Customers -> Sales para evitar violações de FK.
                    if (!(await new ResetProductTablesUsecase(_logger, _context, new object { }).Run()).Ok()) {
                        return Utils.Responses.DefaultInternalServerError<object>();
                    }

                    if (!(await new ResetCustomersTablesUsecase(_logger, _context, new object { }).Run()).Ok()) {
                        return Utils.Responses.DefaultInternalServerError<object>();
                    }

                    if (!(await new ResetSalesTablesUsecase(_logger, _context, new object { }).Run()).Ok()) {
                        return Utils.Responses.DefaultInternalServerError<object>();
                    }

                    // Buscando entidades de seus respectivos endpoints e fazendo um Bulk Insert

                    var prodRes = await client.GetStringAsync(productsUrl);
                    var products = JsonConvert.DeserializeObject<List<ProductInput>>(prodRes.Trim('"').Replace("\\", ""));
                    var prodMapped = products.Select(pr => new Product { Id = pr.idProduto, Description = pr.dscProduto, UnitaryPrice = pr.vlrUnitario }).ToList();
                    var prodResult = await new BulkCreateProductsUsecase(_logger, _context, prodMapped).Run();
                    if (!prodResult.Ok()) {
                        return prodResult;
                    }

                    var custsRes = await client.GetStringAsync(custsUrl);
                    var customers = JsonConvert.DeserializeObject<List<CustomerInput>>(custsRes.Trim('"').Replace("\\", ""));
                    var custMapped = customers.Select(pr => new Customer { Id = pr.idCliente, Name = pr.nmCliente, County = pr.Cidade }).ToList();
                    var custResult = await new BulkCreateCustomersUsecase(_logger, _context, custMapped).Run();
                    if (!custResult.Ok()) {
                        return custResult;
                    }

                    var salesRes = await client.GetStringAsync(salesUrl);
                    var sales = JsonConvert.DeserializeObject<List<SalesInput>>(salesRes.Trim('"').Replace("\\", "").Replace("/", ""));
                    var salesMapped = sales.Select(pr => {
                        long dateTime;
                        try {
                            var milli = pr.dthVenda.Substring(5, pr.dthVenda.Length - 6);
                            dateTime = long.Parse(milli);
                        } catch (Exception ex) {
                            _logger.LogError(ex, "API enviou data invalida.");

                            // DateTime.Now como fallback
                            dateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        }
                        return new Sale {
                            Id = pr.idVenda,
                            CustomerId = pr.idCliente,
                            Date = dateTime,
                            ProductId = pr.idProduto,
                            Quantity = pr.qtdVenda,
                            Total = pr.qtdVenda * pr.vlrUnitarioVenda,
                            UnitaryValue = pr.vlrUnitarioVenda
                        };
                    }).ToList();
                    var salesResult = await new BulkCreateSalesUsecase(_logger, _context, salesMapped).Run();
                    if (!salesResult.Ok()) {
                        return salesResult;
                    }

                    transactionScope.Complete();
                    return new OpResponse<object> {
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
