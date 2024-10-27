using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
