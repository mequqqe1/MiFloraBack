﻿using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }

}
