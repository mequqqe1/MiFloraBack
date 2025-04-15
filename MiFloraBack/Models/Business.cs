using System.ComponentModel.DataAnnotations;
using MiFloraBack.Models;

public class Business
{
    [Key]
    public Guid BusinessId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public Guid OwnerUserId { get; set; }
    public User Owner { get; set; }

    public ICollection<Branch> Branches { get; set; }
}
