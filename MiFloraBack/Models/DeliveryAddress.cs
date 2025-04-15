using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class DeliveryAddress
    {
        [Key]
        public Guid AddressId { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Apartment { get; set; }

        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string DeliveryInstructions { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }

}
