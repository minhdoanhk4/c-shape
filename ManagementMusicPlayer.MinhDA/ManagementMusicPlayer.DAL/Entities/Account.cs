using System;
using System.Collections.Generic;

namespace ManagementMusicPlayer.DAL.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }
}
