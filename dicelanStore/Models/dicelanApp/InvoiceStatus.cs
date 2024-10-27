using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class InvoiceStatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
