using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFloraBack.Data;
using MiFloraBack.Models;
using MiFloraBack.Models.DTOs;
using System.Security.Claims;

namespace MiFloraBack.Controllers
{
    [ApiController]
    [Route("catalog/items")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // 🔍 Получить список товаров
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetItems([FromQuery] string? search, [FromQuery] Guid? category_id, [FromQuery] bool? is_active)
        {
            var userId = GetUserId();
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == userId);
            if (shop == null)
                return Forbid("У вас нет магазина");

            var query = _db.Products
                .Where(p => p.ShopId == shop.ShopId)
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));

            if (category_id.HasValue)
                query = query.Where(p => p.CategoryId == category_id);

            if (is_active.HasValue)
                query = query.Where(p => p.IsActive == is_active);

            var items = await query.ToListAsync();

            return Ok(new
            {
                items = items.Select(p => new
                {
                    id = p.ProductId,
                    name = p.Name,
                    price = p.Price,
                    category_id = p.CategoryId,
                    image_url = p.ImageUrl,
                    is_active = p.IsActive,
                    stock = p.Stock,
                    unit = p.Unit
                }),
                total = items.Count
            });
        }

        [HttpPost("upload")]
        [Authorize(Roles = "owner")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> CreateItemWithImage([FromForm] CreateProductWithImageDto dto)
        {
            var userId = GetUserId();

            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == userId);
            if (shop == null)
                return Forbid("У вас нет магазина");

            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest("Файл изображения обязателен");

            // 📁 Путь до wwwroot/uploads/products
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "products");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var imagePath = $"/uploads/products/{fileName}";

            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Unit = dto.Unit,
                ImageUrl = imagePath,
                IsActive = dto.IsActive,
                CategoryId = dto.CategoryId,
                ShopId = shop.ShopId,
                ExpirationDate = dto.ExpirationDate,
                BatchNumber = dto.BatchNumber
            };

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                id = product.ProductId,
                message = "Товар успешно добавлен"
            });
        }



        // ✏️ Обновить товар
        [HttpPut("{id}")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] UpdateProductDto dto)
        {
            var userId = GetUserId();
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == userId);
            if (shop == null)
                return Forbid("У вас нет магазина");

            var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id && p.ShopId == shop.ShopId);
            if (product == null)
                return NotFound("Товар не найден");

            product.Stock = dto.Stock;
            product.Price = dto.Price;
            product.Unit = dto.Unit;
            product.Description = dto.Description;
            product.IsActive = dto.IsActive;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Товар обновлён", code = 200 });
        }

        // ❌ Удалить товар
        [HttpDelete("{id}")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var userId = GetUserId();
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == userId);
            if (shop == null)
                return Forbid("У вас нет магазина");

            var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id && p.ShopId == shop.ShopId);
            if (product == null)
                return NotFound("Товар не найден");

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Товар удалён", code = 200 });
        }
    }
}
