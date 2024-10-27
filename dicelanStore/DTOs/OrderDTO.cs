using System;
using System.Collections.Generic;

namespace dicelanStore.DTOs
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public double? Total { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public List<OrderProductDTO> Products { get; set; } = new List<OrderProductDTO>();
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class OrderProductDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public byte[]? Image { get; set; }
    }
}
