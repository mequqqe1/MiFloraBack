using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Session
    {
        [Key]
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
    }

}
