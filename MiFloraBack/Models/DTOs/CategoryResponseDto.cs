using System;
using System.Collections.Generic;

namespace MiFloraBack.Models.DTOs
{
    public class CategoryResponseDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }
        public List<CategoryResponseDto> Subcategories { get; set; } = new();
    }
}