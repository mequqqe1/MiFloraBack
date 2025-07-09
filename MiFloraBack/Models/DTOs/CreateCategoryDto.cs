using System;
using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models.DTOs
{
    public class CreateCategoryDto
    {
        public Guid? CategoryId { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }
    }
}