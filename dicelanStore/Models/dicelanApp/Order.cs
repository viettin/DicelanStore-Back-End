using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class Order
{
    public long Id { get; set; }

    public string? Status { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public string? OrderType { get; set; }

    public double? Total { get; set; }

    public long UserId { get; set; }

    public int OrderStatusId { get; set; }

    public int? AddressId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;
}
