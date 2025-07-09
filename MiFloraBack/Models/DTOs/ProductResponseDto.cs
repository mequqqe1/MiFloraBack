using System;

namespace MiFloraBack.Models.DTOs
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public int? Height { get; set; }
        public string Unit { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int Stock { get; set; }
    }
}