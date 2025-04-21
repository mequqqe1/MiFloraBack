using Microsoft.AspNetCore.Http;

namespace MiFloraBack.Models.DTOs
{
    public class CreateProductWithImageDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile Image { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? BatchNumber { get; set; }
    }
}
