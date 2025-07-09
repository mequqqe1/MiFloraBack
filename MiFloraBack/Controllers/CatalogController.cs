using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFloraBack.Data;
using MiFloraBack.Models;
using MiFloraBack.Models.DTOs;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiFloraBack.Controllers
{
    [ApiController]
    [Route("catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CatalogController(ApplicationDbContext db)
        {
            _db = db;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("categories")]
        [Authorize]
        public async Task<IActionResult> GetCategories()
        {
            var userId = GetUserId();
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == userId);
            if (shop == null)
                return Forbid("Ты не владелец магазина");

            var categories = await _db.Categories
                .Where(c => c.ShopId == shop.ShopId && c.ParentId == null)
                .Include(c => c.Subcategories).ThenInclude(sc => sc.Subcategories)
                .Include(c => c.Products)
                .ToListAsync();

            var result = categories.Select(c => new
            {
                id = c.CategoryId,
                name = c.Name,
                parent_id = c.ParentId,
                subcategories = c.Subcategories.Select(sub => new
                {
                    id = sub.CategoryId,
                    name = sub.Name,
                    parent_id = sub.ParentId,
                    subcategories = sub.Subcategories.Select(subsub => new
                    {
                        id = subsub.CategoryId,
                        name = subsub.Name,
                        parent_id = subsub.ParentId,
                        items = subsub.Products.Select(p => new
                        {
                            id = p.ProductId,
                            title = p.Title,
                            price = p.Price,
                            image_url = p.ImageUrl,
                            is_active = p.IsActive,
                            stock = p.Stock,
                            unit = p.Unit
                        })
                    }),
                    items = sub.Products.Select(p => new
                    {
                        id = p.ProductId,
                        title = p.Title,
                        price = p.Price,
                        image_url = p.ImageUrl,
                        is_active = p.IsActive,
                        stock = p.Stock,
                        unit = p.Unit
                    })
                }),
                items = c.Products.Select(p => new
                {
                    id = p.ProductId,
                    title = p.Title,
                    price = p.Price,
                    image_url = p.ImageUrl,
                    is_active = p.IsActive,
                    stock = p.Stock,
                    unit = p.Unit
                })
            });

            return Ok(result);
        }

        [HttpPost("categories")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var userId = GetUserId();
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == userId);
            if (shop == null)
                return Forbid("Ты не владелец ни одного магазина");

            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = dto.Name,
                ParentId = dto.ParentId,
                ShopId = shop.ShopId
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                id = category.CategoryId,
                name = category.Name,
                parent_id = category.ParentId,
                shop_id = category.ShopId
            });
        }
    }
}