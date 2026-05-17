using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Subcription
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Plan Plan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
