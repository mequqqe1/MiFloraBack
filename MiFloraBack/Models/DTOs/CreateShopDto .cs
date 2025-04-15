namespace MiFloraBack.Models.DTOs
{
    public class CreateShopDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Guid OwnerId { get; set; }
    }
}
