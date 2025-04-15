using System.ComponentModel.DataAnnotations;


namespace MiFloraBack.Models
{
    public class Branch
    {
        [Key]
        public Guid BranchId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public Guid BusinessId { get; set; }
        public Business Business { get; set; }

        public ICollection<Shop> Shops { get; set; }
    }

}
