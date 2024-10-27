using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class Store
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Street { get; set; }

    public string? Province { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? City { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
