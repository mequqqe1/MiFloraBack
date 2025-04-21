namespace MiFloraBack.Models.DTOs
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }

}
