using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiFloraBack.Data;
using MiFloraBack.Models;
using MiFloraBack.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiFloraBack.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        private async Task<Shop?> GetUserShop()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var shop = await _db.Shops.FirstOrDefaultAsync(s => s.OwnerId == userId);
            if (shop != null) return shop;
            var userShop = await _db.UserShops
                .Include(us => us.Shop)
                .FirstOrDefaultAsync(us => us.UserId == userId);
            return userShop?.Shop;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var shop = await GetUserShop();
            if (shop == null)
                return Forbid("Вы не привязаны к магазину");

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                ShopId = shop.ShopId,
                ClientName = dto.ClientName,
                Phone = dto.Phone,
                ClientComment = dto.Comment,
                CreatedAt = DateTime.UtcNow,
                Status = "new",
                PaymentStatus = "unpaid",
                DeliveryType = dto.Courier != null ? "courier" : "self_pickup",
                Address = dto.Courier?.Address,
                DeliveryTime = dto.Courier?.DeliveryTime ?? dto.SelfPickup?.SelfPickupTime
            };

            var orderItems = new List<OrderItem>();
            decimal total = 0m;

            foreach (var item in dto.Items)
            {
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product == null)
                    return NotFound($"Товар {item.ProductId} не найден");

                total += product.Price * item.Quantity;
                orderItems.Add(new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            order.Price = (float)total;  // explicit cast to float
            order.OrderItems = orderItems;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, new { order_id = order.OrderId });
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string? status,
                                                  [FromQuery] string? search,
                                                  [FromQuery] DateTime? date_from,
                                                  [FromQuery] DateTime? date_to)
        {
            var query = _db.Orders.AsQueryable();
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(o => o.Status == status);
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(o => o.ClientName.Contains(search) || o.Phone.Contains(search));
            if (date_from.HasValue)
                query = query.Where(o => o.CreatedAt >= date_from.Value);
            if (date_to.HasValue)
                query = query.Where(o => o.CreatedAt <= date_to.Value);

            var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
            return Ok(orders.Select(o => new
            {
                order_id = o.OrderId,
                client_name = o.ClientName,
                phone = o.Phone,
                delivery_type = o.DeliveryType,
                price = o.Price,
                status = o.Status,
                created_at = o.CreatedAt
            }));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null) return NotFound();

            return Ok(new
            {
                id = order.OrderId,
                client_name = order.ClientName,
                phone = order.Phone,
                address = order.Address,
                client_comment = order.ClientComment,
                status = order.Status,
                payment_status = order.PaymentStatus,
                delivery_time = order.DeliveryTime,
                items = order.OrderItems.Select(oi => new
                {
                    product_id = oi.ProductId,
                    title = oi.Product.Title,
                    quantity = oi.Quantity,
                    price = oi.Product.Price
                }),
                total = order.Price
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto dto)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = dto.Status ?? order.Status;
            order.PaymentStatus = dto.PaymentStatus ?? order.PaymentStatus;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Заказ обновлён" });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Заказ удалён" });
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var today = DateTime.UtcNow.Date;
            var totalToday = await _db.Orders.CountAsync(o => o.CreatedAt.Date == today);
            var totalAmount = await _db.Orders.SumAsync(o => (float?)o.Price) ?? 0f;
            var delivered = await _db.Orders.CountAsync(o => o.Status == "delivered");
            var cancelled = await _db.Orders.CountAsync(o => o.Status == "cancelled");
            var inProgress = await _db.Orders.CountAsync(o => o.Status == "in_progress");

            return Ok(new
            {
                total_today = totalToday,
                total_amount = totalAmount,
                delivered,
                cancelled,
                in_progress = inProgress
            });
        }
    }
}
