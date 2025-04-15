using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFloraBack.Data;
using MiFloraBack.Models;
using MiFloraBack.Models.DTOs;
using MiFloraBack.Service;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;


    private readonly JwtService _jwt;

    public AuthController(ApplicationDbContext db, IPasswordHasher<User> hasher, JwtService jwt)
    {
        _db = db;
        _passwordHasher = hasher;
        _jwt = jwt;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Phone == dto.Phone))
            return BadRequest("Пользователь с таким телефоном уже существует");

        var user = new User
        {
            UserId = Guid.NewGuid(),
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = dto.Email,
            Password = ""
        };

        user.Password = _passwordHasher.HashPassword(user, dto.Password);
        await _db.Users.AddAsync(user);

        // 👇 Присваиваем роль "owner"
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role);
        if (role != null)
        {
            await _db.UserRoles.AddAsync(new UserRole
            {
                UserRoleId = Guid.NewGuid(),
                UserId = user.UserId,
                RoleId = role.RoleId
            });
        }

        await _db.SaveChangesAsync();

        var token = _jwt.GenerateToken(user, dto.Role);

        return Ok(new
        {
            user_id = user.UserId,
            full_name = user.FullName,
            phone = user.Phone,
            role = dto.Role,
            token = token
        });
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Phone == dto.Phone);
        if (user == null)
            return Unauthorized();

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized();

        var role = await _db.UserRoles.Include(x => x.Role)
            .Where(x => x.UserId == user.UserId)
            .Select(x => x.Role.Name)
            .FirstOrDefaultAsync();

        var token = _jwt.GenerateToken(user, role);

        return Ok(new
        {
            user_id = user.UserId,
            full_name = user.FullName,
            role = role,
            email = user.Email,
            token = token
        });
    }



    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _db.Users.FindAsync(Guid.Parse(userId));
        if (user == null) return Unauthorized();

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.OldPassword);
        if (result == PasswordVerificationResult.Failed)
            return BadRequest("Неверный старый пароль");

        user.Password = _passwordHasher.HashPassword(user, dto.NewPassword);
        await _db.SaveChangesAsync();

        return Ok("Пароль обновлён");
    }

}
