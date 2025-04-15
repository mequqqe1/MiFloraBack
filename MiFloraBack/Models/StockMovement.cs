using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class StockMovement
    {
        [Key]
        public Guid MovementId { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public string Type { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public string Reason { get; set; }
    }

}
