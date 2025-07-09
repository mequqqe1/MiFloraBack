using System;
using Microsoft.AspNetCore.Http;

namespace MiFloraBack.Models.DTOs
{
    public class UpdateProductDto
    {
        public string? Title { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Height { get; set; }
        public string? Unit { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public int? Stock { get; set; }
    }
}