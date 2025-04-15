using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Supplier
    {
        [Key]
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; }
    }

}
