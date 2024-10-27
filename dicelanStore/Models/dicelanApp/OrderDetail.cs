using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class OrderDetail
{
    public int Id { get; set; }

    public short? Quantity { get; set; }

    public double? UnitPrice { get; set; }

    public double? Discount { get; set; }

    public double? Total { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public int ProductId { get; set; }

    public long? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product Product { get; set; } = null!;
}
