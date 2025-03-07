using backend.Model;
using backend.Repository;

namespace backend.Usecase {
    public class CriarCliente: UsecaseBase<ClientesRepository, CriarCliente.Input, Cliente> {

        public class Input {
            private string nmCliente { get; set; }
            private string cidade { get; set; }
        }

        public OpResponse<Cliente> Run(ClientesRepository rep, Input input) {
            throw new NotImplementedException();
        }


    }
}
