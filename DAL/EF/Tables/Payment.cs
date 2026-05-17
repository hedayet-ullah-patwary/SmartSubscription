using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Payment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SubcriptionId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public virtual Subcription Subcription { get; set; } = null!;
}
