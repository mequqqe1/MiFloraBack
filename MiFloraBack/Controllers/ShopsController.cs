using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiFloraBack.Data;
using MiFloraBack.Models.DTOs;
using MiFloraBack.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MiFloraBack.Controllers
{
    [ApiController]
    [Route("api/shops")]
    public class ShopsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _http;

        public ShopsController(ApplicationDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        private async Task<bool> IsOwnerOfShop(Guid shopId)
        {
            var userId = GetUserId();
            return await _db.Shops.AnyAsync(s => s.ShopId == shopId && s.OwnerId == userId);
        }

        // 5. Создание магазина
        [HttpPost("create")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> CreateShop(CreateShopDto dto)
        {
            var ownerId = GetUserId();

            // Проверка: у owner может быть только 1 магазин (опционально)
            var alreadyExists = await _db.Shops.AnyAsync(s => s.OwnerId == ownerId);
            if (alreadyExists)
                return BadRequest("Магазин уже создан");

            var shop = new Shop
            {
                ShopId = Guid.NewGuid(),
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                WorkingHours = "09:00-21:00",
                OwnerId = ownerId
            };

            await _db.Shops.AddAsync(shop);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                shop_id = shop.ShopId,
                name = shop.Name,
                address = shop.Address,
                phone = shop.Phone,
                owner_id = shop.OwnerId
            });
        }

        // 6. Получение информации
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetShop(Guid id)
        {
            var shop = await _db.Shops
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(s => s.ShopId == id);

            if (shop == null) return NotFound();

            return Ok(new
            {
                shop_id = shop.ShopId,
                name = shop.Name,
                address = shop.Address,
                phone = shop.Phone,
                working_hours = shop.WorkingHours,
                owner_id = shop.OwnerId,
                owner_name = shop.Owner.FullName
            });
        }

        // 7. Обновление магазина (только если ты — его owner)
        [HttpPut("{id}")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> UpdateShop(Guid id, UpdateShopDto dto)
        {
            var shop = await _db.Shops.FindAsync(id);
            if (shop == null) return NotFound();

            var userId = GetUserId();
            if (shop.OwnerId != userId)
                return Forbid("Ты не владелец этого магазина");

            shop.Name = dto.Name;
            shop.Address = dto.Address;
            shop.Phone = dto.Phone;
            shop.WorkingHours = dto.WorkingHours;

            await _db.SaveChangesAsync();
            return Ok(shop);
        }
    }

}
