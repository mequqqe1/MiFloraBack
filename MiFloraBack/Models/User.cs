using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Session> Sessions { get; set; }
        public ICollection<UserShop> UserShops { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Loyalty> Loyalties { get; set; }
        public ICollection<DeliveryStatus> DeliveryStatusesAsCourier { get; set; }
    }

}
