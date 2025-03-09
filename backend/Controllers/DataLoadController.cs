using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers {

    [ApiController]
    [Route("/dataLoad")]
    public class DataLoadController : ControllerBase {

        [HttpPost("/clientes")]
        public OpResponse<object> LoadCustomers() {
            return new OpResponse<object> {
                Status = 200,
                Message = "Data loaded successfully",
            };
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
