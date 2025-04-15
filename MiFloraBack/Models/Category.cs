using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        public Category ParentCategory { get; set; }

        public ICollection<Category> Subcategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }

}
