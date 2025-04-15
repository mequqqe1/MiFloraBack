using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public float Amount { get; set; }
        public string Method { get; set; }
        public DateTime PaidAt { get; set; }
    }

}
