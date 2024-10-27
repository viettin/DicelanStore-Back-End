using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class Image
{
    public int Id { get; set; }

    public byte[]? Image1 { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
