using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models.DTOs;

public class CreateOrderDto
{
    [Required]
    public string ClientName { get; set; }

    [Required]
    public string Phone { get; set; }

    public string? Comment { get; set; }

    public CourierDeliveryDto? Courier { get; set; }

    public SelfPickupDto? SelfPickup { get; set; }

    [Required]
    public List<OrderItemDto> Items { get; set; }
}

public class CourierDeliveryDto
{
    public string Address { get; set; }

    public DateTime DeliveryTime { get; set; }
}

public class SelfPickupDto
{
    public DateTime? SelfPickupTime { get; set; }
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
