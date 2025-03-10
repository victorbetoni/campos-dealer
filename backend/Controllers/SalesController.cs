using backend.Context;
using backend.Model;
using backend.Usecase.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Globalization;
using System.Reflection.Emit;

namespace backend.Controllers;

[ApiController]
[Route("/venda")]
public class SalesController : ControllerBase {

    private readonly ILogger<SalesController> _logger;
    private ApiDbContext _context;

    public SalesController(ILogger<SalesController> logger, ApiDbContext context) {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public async Task<OpResponse<Sale>> Post([FromBody] CreateSaleUsecase.Input sale) {

        // Poderia utilizar um middleware para fazer essa validação

        if (!ModelState.IsValid) {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
            return Utils.Responses.DefaultFillAllFields<Sale>(errors);
        }

        if (sale.Quantity < 1) {
            return new OpResponse<Sale> {
                Status = 400,
                Message = "Quantidade deve maior ou igual a 1"
            };
        }

        var res = await new CreateSaleUsecase(_logger, _context, sale).Run();
        HttpContext.Response.StatusCode = res.Status;
        return res;
    }

    [HttpGet]
    public async Task<OpResponse<List<Sale>>> Get() {
        var nameFilter = Request.Query["name"];
        var descFilter = Request.Query["desc"];

        var page = Utils.QueryOrDefaultInt(HttpContext, "page", 1);

        var input = new ListSalesUsecase.Input{
            NameFilter = nameFilter,
            DescFilter = descFilter,
            Page = page
        };

        var res = await new ListSalesUsecase(_logger, _context, input).Run();
        HttpContext.Response.StatusCode = res.Status;
        return res;
    }

    [HttpPut]
    public async Task<OpResponse<Sale>> Put([FromBody] Sale sale) {

        if (!ModelState.IsValid) {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
            return Utils.Responses.DefaultFillAllFields<Sale>(errors);
        }

        // Validação dos dados primitivos nessa camada, mas a validação das FK
        // é por conta da camada de negócio.

        // Validação do DateTime é realizado pelo proprio middleware do .NET
        // Validação do Total não é necessário pois o caso de uso recalcula automaticamente.

        if(sale.Quantity < 1) {
            return new OpResponse<Sale> {
                Status = 400,
                Message = "Quantidade deve maior ou igual a 1"
            };
        }

        var res = await new UpdateSaleUsecase(_logger, _context, sale).Run();
        HttpContext.Response.StatusCode = res.Status;
        return res;
    }

    [HttpDelete]
    public async Task<OpResponse<object>> Delete([FromBody] DeleteSaleUsecase.Input id)  {
        var res = await new DeleteSaleUsecase(_logger, _context, id).Run();
        HttpContext.Response.StatusCode = res.Status;
        return res;
    }
}
