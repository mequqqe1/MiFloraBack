using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MiFloraBack.Models.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public Guid CategoryId { get; set; }

        public int? Height { get; set; }
        [Required]
        public string Unit { get; set; } = null!;

        public string? Description { get; set; }
        public IFormFile? Image { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}