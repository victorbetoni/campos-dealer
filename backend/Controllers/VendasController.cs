using backend.Model;
using backend.Usecase;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("/Sites/TesteAPI/venda")]
public class VendasController : ControllerBase
{

    private readonly ILogger<VendasController> _logger;

    public VendasController(ILogger<VendasController> logger) {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Venda> Get() {
        ListarVendas.Input input = new ListarVendas.Input {
            cliente = Request.Query["cliente"],
            descProduto = Request.Query["descProduto"]
        };
        IEnumerable<Venda> res = new ListarVendas().Run(null, input);
        return res;
    }
}
