using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class Cart
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public long UserId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
