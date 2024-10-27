﻿using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class CartItem
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
