using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class Invoice
{
    public long Id { get; set; }

    public double? SubTotal { get; set; }

    public double? TaxAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public double? Total { get; set; }

    public DateTime CreatedDate { get; set; }

    public long OrderId { get; set; }

    public int InvoiceStatusId { get; set; }

    public virtual InvoiceStatus InvoiceStatus { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
