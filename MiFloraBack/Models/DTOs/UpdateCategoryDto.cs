using System;

namespace MiFloraBack.Models.DTOs
{
    public class UpdateCategoryDto
    {
        public string? Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}