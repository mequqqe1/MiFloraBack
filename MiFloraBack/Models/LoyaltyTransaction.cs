using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class LoyaltyTransaction
    {
        [Key]
        public Guid TransactionId { get; set; }

        public Guid LoyaltyId { get; set; }
        public Loyalty Loyalty { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public int Points { get; set; }
        public string Type { get; set; } // earned / spent
        public DateTime CreatedAt { get; set; }
    }

}
