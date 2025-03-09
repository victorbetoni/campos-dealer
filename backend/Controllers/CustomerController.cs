using backend.Context;
using backend.Model;
using backend.Repository;
using backend.Usecase.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;

namespace backend.Controllers;

[ApiController]
[Route("/Sites/TesteAPI/cliente")]
public class CustomerController : ControllerBase {

    private readonly CustomerRepository _clienteRepository;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ILogger<CustomerController> logger, ApiDbContext context) {
        _logger = logger;
        this._clienteRepository = new CustomerRepository(context);
    }

    [HttpPost]
    public OpResponse<Customer> Post([FromBody] Customer cliente) {
        // Poderia utilizar um middleware para fazer essa validação
        if (!ModelState.IsValid) {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
            return new OpResponse<Customer>() {
                Status = 400,
                Errors = errors,
                Message = "Preencha todos os campos corretamente."
            };
        }

        var input = new CreateCustomerUsecase.Input {
            Name = cliente.Name,
            County = cliente.County
        };

        if (string.IsNullOrEmpty(input.Name) || string.IsNullOrEmpty(input.County)) {
            return new OpResponse<Customer> {
                Status = 400,
                Message = "Preencha todos os campos corretamente."
            };
        }

        var res = new CreateCustomerUsecase(_logger, _clienteRepository, input).Run();
        return res;
    }

    [HttpGet]
    public OpResponse<List<Customer>> Get() {
        var nameFilter = Request.Query["name"];
        var page = 1;
        if (!Request.Query["page"].IsNullOrEmpty()){
            try {
                page = int.Parse(Request.Query["page"]);
            } catch (Exception ex) {
                page = 1;
            }
        }
        var input = new ListCustomerUsecase.Input{
            NameFilter = nameFilter,
            Page = page
        };
        var res = new ListCustomerUsecase(_logger, _clienteRepository, input).Run();
        return res;
    }

    [HttpPut]
    public OpResponse<Customer> Put([FromBody] Customer cliente) {
        
        if (!ModelState.IsValid) {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
            return new OpResponse<Customer>() {
                Status = 400,
                Errors = errors,
                Message = "Preencha todos os campos corretamente."
            };
        }

        if (string.IsNullOrEmpty(cliente.Name) || string.IsNullOrEmpty(cliente.County)) {
            return new OpResponse<Customer> {
                Status = 400,
                Message = "Preencha todos os campos corretamente."
            };
        }


        var res = new UpdateCustomerUsecase(_logger, _clienteRepository, cliente).Run();
        return res;
    }

    [HttpDelete]
    public OpResponse<object> Delete([FromBody] DeleteCustomerUsecase.Input id)  {
        var res = new DeleteCustomerUsecase(_logger, _clienteRepository, id).Run();
        return res;
    }
}
