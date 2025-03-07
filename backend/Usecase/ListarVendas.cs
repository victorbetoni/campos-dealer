using backend.Model;
using backend.Repository;

namespace backend.Usecase {
    public class ListarVendas: UsecaseBase<VendasRepository, ListarVendas.Input, List<Venda>> {

        public class Input{
            public string? cliente { get; set; }
            public string? descProduto { get; set; }
        }

        public List<Venda> Run(VendasRepository rep, Input input) {
            throw new NotImplementedException();
        }


    }
}
