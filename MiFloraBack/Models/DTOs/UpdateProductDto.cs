namespace MiFloraBack.Models.DTOs
{
    public class UpdateProductDto
    {
        public int Stock { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
