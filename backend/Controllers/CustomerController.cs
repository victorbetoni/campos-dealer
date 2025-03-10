using backend.Context;
using backend.Model;
using backend.Repository;
using backend.Usecase.Customers;
using backend.Usecase.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using System.Threading.Tasks;

namespace backend.Controllers;

[ApiController]
[Route("/cliente")]
public class CustomerController : ControllerBase {

    private readonly ILogger<CustomerController> _logger;
    private ApiDbContext _context;

    public CustomerController(ILogger<CustomerController> logger, ApiDbContext context) {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public async Task<OpResponse<Customer>> Post([FromBody] CreateCustomerUsecase.Input cliente) {
        // Poderia utilizar um middleware para fazer essa validação
        if (!ModelState.IsValid) {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
            return Utils.Responses.DefaultFillAllFields<Customer>(errors);
        }

        if (!Utils.AllFilled(cliente.Name, cliente.County)) {
            return Utils.Responses.DefaultFillAllFields<Customer>();
        }

        var res = await new CreateCustomerUsecase(_logger, _context, cliente).Run();
        HttpContext.Response.StatusCode = res.Status;
        return res;
    }

    [HttpGet]
    public async Task<OpResponse<List<Customer>>> Get() {
        var nameFilter = Request.Query["name"];
        
        var page = Utils.QueryOrDefaultInt(HttpContext, "page", 1);

        var input = new ListCustomerUsecase.Input{
            NameFilter = nameFilter,
            Page = page
        };

        var res = await new ListCustomerUsecase(_logger, _context, input).Run();
        HttpContext.Response.StatusCode = res.Status;
        return res;
    }

    [HttpPut]
    public async Task<OpResponse<Customer>> Put([FromBody] Customer cliente) {

        if (!ModelState.IsValid) {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
            return Utils.Responses.DefaultFillAllFields<Customer>(errors);
        }

        if (!Utils.AllFilled(cliente.Name, cliente.County)) {
            return Utils.Responses.DefaultFillAllFields<Customer>();
        }

        var res = await new UpdateCustomerUsecase(_logger, _context, cliente).Run();HttpContext.Response.StatusCode = res.Status;
        HttpContext.Response.StatusCode = res.Status;
        return res;
    }

    [HttpDelete]
    public async Task<OpResponse<object>> Delete([FromBody] DeleteCustomerUsecase.Input id)  {
        var res = await new DeleteCustomerUsecase(_logger, _context, id).Run();
        HttpContext.Response.StatusCode = res.Status;
        return res;
    }
}
