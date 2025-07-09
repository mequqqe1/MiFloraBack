using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;

        public Guid? ParentId { get; set; }
        public Category? Parent { get; set; }
        public ICollection<Category> Subcategories { get; set; } = new List<Category>();

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}