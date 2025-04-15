using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Subscription
    {
        [Key]
        public Guid SubscriptionId { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid DeliveryAddressId { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }

        public string Frequency { get; set; } // weekly, monthly
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Quantity { get; set; }
        public string Status { get; set; } // active, canceled
    }

}
