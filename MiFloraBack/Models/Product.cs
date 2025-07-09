using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }

        [Required]
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;

        [Required]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;      // Название

        public int? Height { get; set; }               // Высота (для отдельных категорий)

        [Required]
        public string Unit { get; set; } = null!;       // Единица измерения (шт, банч)

        public string? Description { get; set; }        // Описание
        public string? ImageUrl { get; set; }           // URL изображения

        [Required]
        public decimal Price { get; set; }              // Цена (продажа)

        public bool IsActive { get; set; } = true;      // Активность товара
        public int Stock { get; set; } = 0;             // Остаток на складе

        // Дополнительные отношения
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public ICollection<Spoilage> Spoilages { get; set; } = new List<Spoilage>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    }
}