using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Loyalty
    {
        [Key]
        public Guid LoyaltyId { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int TotalEarnedPoints { get; set; }
        public int TotalSpentPoints { get; set; }

        public ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; }
    }

}
