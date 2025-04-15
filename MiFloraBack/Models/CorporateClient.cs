using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class CorporateClient
    {
        [Key]
        public Guid ClientId { get; set; }

        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public float DiscountPercentage { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

}
