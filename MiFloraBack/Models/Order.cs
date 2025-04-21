using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }

        // 👤 Клиент
        [Required]
        public string ClientName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? ClientComment { get; set; } // 🔧 fix

        // 🚚 Доставка
        [Required]
        public string DeliveryType { get; set; } // courier | self_pickup

        public string? Address { get; set; }
        public DateTime? DeliveryTime { get; set; }

        // 👤 Доп. инфа
        public string? FloristName { get; set; } // 🔧 fix

        // 💰 Финансы
        public float Price { get; set; } = 0; // 🔧 fix
        public string Status { get; set; } = "new";
        public string PaymentStatus { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 📦 Состав заказа
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
