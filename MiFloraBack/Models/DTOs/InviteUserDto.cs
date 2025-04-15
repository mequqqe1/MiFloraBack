namespace MiFloraBack.Models.DTOs
{
    public class InviteUserDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }  // 👈 новое поле
    }

}

