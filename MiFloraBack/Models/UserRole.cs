using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class UserRole
    {
        [Key]
        public Guid UserRoleId { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Guid? BusinessId { get; set; }
        public Business Business { get; set; }

        public Guid? BranchId { get; set; }
        public Branch Branch { get; set; }

        public Guid? ShopId { get; set; }
        public Shop Shop { get; set; }
    }

}
