using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public string BatchNumber { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; }
        public ICollection<Spoilage> Spoilages { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }

}
