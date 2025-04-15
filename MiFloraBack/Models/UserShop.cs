using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class UserShop
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }
    }

}
