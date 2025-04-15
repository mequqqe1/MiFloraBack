using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Holiday
    {
        [Key]
        public Guid HolidayId { get; set; }

        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsRecurring { get; set; }
    }

}
