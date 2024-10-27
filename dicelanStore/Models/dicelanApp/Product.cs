using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double? Price { get; set; }

    public short? StockQuantity { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public int MaterialId { get; set; }

    public int ProductTypeId { get; set; }

    public decimal? Discount { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Material Material { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ProductType ProductType { get; set; } = null!;
}
