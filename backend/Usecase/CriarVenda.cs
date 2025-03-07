using backend.Model;
using backend.Repository;

namespace backend.Usecase {
    public class CriarVenda: UsecaseBase<VendasRepository, CriarVenda.Input, Venda> {
        public class Input{
            public string? cliente { get; set; }
            public string? descProduto { get; set; }
        }

        public OpResponse<Venda> Run(VendasRepository rep, Input input) {
            try {

            } catch(Exception ex) {
                return new OpResponse<Venda>(StatusCodes.Status500InternalServerError, "Algo deu errado! Tente novamente daqui a pouco.", null);
            }
        }
    }
}
