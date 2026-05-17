using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IsEmailVerified { get; set; }

    public DateTime CreatedAt { get; set; }

    public int IsActive { get; set; }

    public virtual ICollection<Subcription> Subcriptions { get; set; } = new List<Subcription>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
