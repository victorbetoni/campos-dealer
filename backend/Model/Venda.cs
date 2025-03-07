using Microsoft.AspNetCore.Mvc;

namespace backend.Model
{
    public class Venda
    {
        public int idVenda { get; set; }
        public int idCliente { get; set; }
        public int idProduto { get; set; }
        public int qtdVenda { get; set; }
        public double vlrUnitarioVenda { get; set; }
        public DateTime dthVenda { get; set; }
        public decimal vlrTotalVenda { get; set; }
    }
}
