using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class DeliveryStatus
    {
        [Key]
        public Guid StatusId { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid CourierId { get; set; }
        public User Courier { get; set; }
    }

}
