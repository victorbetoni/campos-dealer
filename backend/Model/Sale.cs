using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model
{
    public class Sale
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitaryValue { get; set; }
        public long Date { get; set; }
        public float Total { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
