using backend.Migrations;
using Microsoft.AspNetCore.Mvc;

namespace backend.Model
{
    public class Sale
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitaryValue { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
