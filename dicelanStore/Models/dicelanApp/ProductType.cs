using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class ProductType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
