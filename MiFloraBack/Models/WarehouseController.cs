// Controllers/WarehouseController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFloraBack.Data;
using MiFloraBack.Models;
using MiFloraBack.Models.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiFloraBack.Controllers
{
    [ApiController]
    [Route("api/warehouse")]
    public class WarehouseController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public WarehouseController(ApplicationDbContext db) => _db = db;

        // GET: api/warehouse/contractors
        [HttpGet("contractors")]
        [Authorize]
        public async Task<IActionResult> GetContractors()
        {
            var list = await _db.Contractors
                .Select(c => new ContractorDto { Id = c.ContractorId, Name = c.Name })
                .ToListAsync();
            return Ok(list);
        }

        // POST: api/warehouse/contractors
        [HttpPost("contractors")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> CreateContractor([FromBody] CreateContractorDto dto)
        {
            var contractor = new Contractor { ContractorId = Guid.NewGuid(), Name = dto.Name.Trim() };
            _db.Contractors.Add(contractor);
            await _db.SaveChangesAsync();
            return Ok(new { id = contractor.ContractorId, name = contractor.Name });
        }

        // GET: api/warehouse/products
        [HttpGet("products")]
        [Authorize]
        public async Task<IActionResult> GetProductsForInvoice()
        {
            var list = await _db.Products
                .Select(p => new InvoiceProductDto
                {
                    Id = p.ProductId,
                    Title = p.Title,
                    AvailableStock = p.Stock,
                    Unit = p.Unit
                })
                .ToListAsync();
            return Ok(list);
        }

        // POST: api/warehouse/invoices
        [HttpPost("invoices")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto)
        {
            // Проверка сущностей
            if (!await _db.Contractors.AnyAsync(c => c.ContractorId == dto.ContractorId))
                return BadRequest("Contractor not found");
            if (!await _db.Branches.AnyAsync(b => b.BranchId == dto.BranchId))
                return BadRequest("Branch not found");

            var invoice = new Invoice
            {
                InvoiceId = dto.Id,
                Date = dto.Date,
                ContractorId = dto.ContractorId,
                BranchId = dto.BranchId
            };
            _db.Invoices.Add(invoice);

            foreach (var item in dto.Items)
            {
                var prod = await _db.Products.FindAsync(item.ProductId);
                if (prod == null)
                    return BadRequest($"Product {item.ProductId} not found");

                // обновляем остаток
                prod.Stock += item.Quantity;

                var invItem = new InvoiceItem
                {
                    InvoiceItemId = Guid.NewGuid(),
                    InvoiceId = dto.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    PurchasePrice = item.PurchasePrice,
                    Total = item.Quantity * item.PurchasePrice
                };
                _db.InvoiceItems.Add(invItem);
            }

            await _db.SaveChangesAsync();
            return Ok(new { status = "success", invoiceId = dto.Id });
        }
    }
}
