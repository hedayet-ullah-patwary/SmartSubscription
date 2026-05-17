using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Plan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public int ApiLimit { get; set; }

    public string Description { get; set; } = null!;

    public int IsActive { get; set; }

    public virtual ICollection<Subcription> Subcriptions { get; set; } = new List<Subcription>();
}
