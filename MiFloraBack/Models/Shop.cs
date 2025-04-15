using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Shop
    {
        [Key]
        public Guid ShopId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        public string WorkingHours { get; set; }

        // 🔗 Связь с филиалом (необязательна, если ты пока не используешь Branch)
        public Guid? BranchId { get; set; }
        public Branch? Branch { get; set; }

        // 🔐 Владелец магазина
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        // ⛓️ Навигационные свойства
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public ICollection<UserShop> UserShops { get; set; } = new List<UserShop>();
    }
}
