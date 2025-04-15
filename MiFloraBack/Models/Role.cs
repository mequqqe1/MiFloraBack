using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Scope { get; set; }
        public string Description { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }

}
