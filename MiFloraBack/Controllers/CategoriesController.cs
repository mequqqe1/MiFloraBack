using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFloraBack.Data;
using MiFloraBack.Models;
using MiFloraBack.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiFloraBack.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public CategoriesController(ApplicationDbContext db) => _db = db;

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var roots = await _db.Categories
                .Where(c => c.ParentId == null)
                .Include(c => c.Subcategories)
                    .ThenInclude(sc => sc.Subcategories)
                .ToListAsync();

            List<CategoryResponseDto> Map(IEnumerable<Category> list)
            {
                return list.Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentId = c.ParentId,
                    Subcategories = Map(c.Subcategories)
                }).ToList();
            }

            return Ok(Map(roots));
        }

        // GET: api/categories/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var cat = await _db.Categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (cat == null)
                return NotFound();

            List<CategoryResponseDto> Map(IEnumerable<Category> list)
            {
                return list.Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentId = c.ParentId,
                    Subcategories = new List<CategoryResponseDto>()
                }).ToList();
            }

            var response = new CategoryResponseDto
            {
                CategoryId = cat.CategoryId,
                Name = cat.Name,
                ParentId = cat.ParentId,
                Subcategories = Map(cat.Subcategories)
            };

            return Ok(response);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");

            var exists = await _db.Categories
                .AnyAsync(c => c.Name == dto.Name && c.ParentId == dto.ParentId);
            if (exists)
                return Conflict("Категория с таким именем уже существует");

            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = dto.Name,
                ParentId = dto.ParentId
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, new
            {
                category_id = category.CategoryId,
                name = category.Name,
                parent_id = category.ParentId
            });
        }

        // PUT: api/categories/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryDto dto)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Name))
                category.Name = dto.Name;
            category.ParentId = dto.ParentId;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await _db.Categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
                return NotFound();

            if (category.Subcategories.Any())
                return BadRequest("Нельзя удалить категорию с подкатегориями");

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
