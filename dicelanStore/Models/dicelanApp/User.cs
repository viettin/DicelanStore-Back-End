using System;
using System.Collections.Generic;

namespace dicelanStore.Models.dicelanApp;

public partial class User
{
    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public byte[]? Avatar { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public bool? IsDeleted { get; set; }

    public int RoleId { get; set; }

    public long Id { get; set; }

    public virtual Role Role { get; set; } = null!;
}
