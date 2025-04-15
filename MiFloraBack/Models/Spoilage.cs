using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Spoilage
    {
        [Key]
        public Guid SpoilageId { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
    }

}
