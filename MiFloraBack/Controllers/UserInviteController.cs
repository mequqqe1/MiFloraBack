using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFloraBack.Data;
using MiFloraBack.Models;
using MiFloraBack.Models.DTOs;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher<User> _hasher;

    public UsersController(ApplicationDbContext db, IPasswordHasher<User> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [HttpPost("/api/users/invite")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> Invite(InviteUserDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Phone == dto.Phone))
            return BadRequest("Пользователь с таким телефоном уже существует");

        var user = new User
        {
            UserId = Guid.NewGuid(),
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = "",
            Password = _hasher.HashPassword(null, dto.Password)
        };

        await _db.Users.AddAsync(user);

        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role);
        if (role == null)
            return BadRequest("Роль не найдена");

        await _db.UserRoles.AddAsync(new UserRole
        {
            UserRoleId = Guid.NewGuid(),
            UserId = user.UserId,
            RoleId = role.RoleId
        });

        await _db.SaveChangesAsync();

        return Ok(new
        {
            user_id = user.UserId,
            full_name = user.FullName,
            phone = user.Phone,
            role = dto.Role
        });
    }


}
