using MiFloraBack.Models;
using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public Guid CategoryId { get; set; }
    public string Name { get; set; }

    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category> Subcategories { get; set; } = new List<Category>();

    public Guid ShopId { get; set; }
    public Shop Shop { get; set; }

    // ✅ добавь это
    public ICollection<Product> Products { get; set; } = new List<Product>();
}



