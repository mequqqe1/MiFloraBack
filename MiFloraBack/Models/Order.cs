using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }

        public Guid? DeliveryAddressId { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }

        public Guid? ClientId { get; set; }
        public CorporateClient CorporateClient { get; set; }

        public bool IsCorporate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryTimeSlot { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<DeliveryStatus> DeliveryStatuses { get; set; }
        public ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; }
    }

}
