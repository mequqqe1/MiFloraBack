using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFloraBack.Data;
using MiFloraBack.Models;
using MiFloraBack.Models.DTOs;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiFloraBack.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? categoryId, [FromQuery] string? search)
        {
            var query = _db.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Title.Contains(search));

            var list = await query.ToListAsync();

            var result = list.Select(p => new ProductResponseDto
            {
                Id = p.ProductId,
                Title = p.Title,
                CategoryId = p.CategoryId,
                Height = p.Height,
                Unit = p.Unit,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                IsActive = p.IsActive,
                Stock = p.Stock
            });

            return Ok(result);
        }

        // GET: api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();

            var dto = new ProductResponseDto
            {
                Id = p.ProductId,
                Title = p.Title,
                CategoryId = p.CategoryId,
                Height = p.Height,
                Unit = p.Unit,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                IsActive = p.IsActive,
                Stock = p.Stock
            };
            return Ok(dto);
        }

        // POST: api/products
        [HttpPost]
        [Authorize(Roles = "owner")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
        {
            if (!await _db.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId))
                return BadRequest("Категория не найдена");

            string? imagePath = null;
            if (dto.Image != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads/products");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var full = Path.Combine(uploads, fileName);
                using var fs = new FileStream(full, FileMode.Create);
                await dto.Image.CopyToAsync(fs);
                imagePath = $"/uploads/products/{fileName}";
            }

            var prod = new Product
            {
                ProductId = Guid.NewGuid(),
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                Height = dto.Height,
                Unit = dto.Unit,
                Description = dto.Description,
                ImageUrl = imagePath,
                Price = dto.Price,
                IsActive = true,
                Stock = 0
            };

            _db.Products.Add(prod);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = prod.ProductId }, new { id = prod.ProductId });
        }

        // PUT: api/products/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateProductDto dto)
        {
            var prod = await _db.Products.FindAsync(id);
            if (prod == null) return NotFound();

            if (dto.Title != null) prod.Title = dto.Title;
            if (dto.CategoryId.HasValue) prod.CategoryId = dto.CategoryId.Value;
            if (dto.Height.HasValue) prod.Height = dto.Height;
            if (dto.Unit != null) prod.Unit = dto.Unit;
            if (dto.Description != null) prod.Description = dto.Description;
            if (dto.Price.HasValue) prod.Price = dto.Price.Value;
            if (dto.IsActive.HasValue) prod.IsActive = dto.IsActive.Value;
            if (dto.Stock.HasValue) prod.Stock = dto.Stock.Value;

            if (dto.Image != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads/products");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var full = Path.Combine(uploads, fileName);
                using var fs = new FileStream(full, FileMode.Create);
                await dto.Image.CopyToAsync(fs);
                prod.ImageUrl = $"/uploads/products/{fileName}";
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var prod = await _db.Products.FindAsync(id);
            if (prod == null) return NotFound();
            _db.Products.Remove(prod);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
