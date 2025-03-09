using Microsoft.AspNetCore.Mvc;

namespace backend.Model
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitaryValue { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
    }
}
