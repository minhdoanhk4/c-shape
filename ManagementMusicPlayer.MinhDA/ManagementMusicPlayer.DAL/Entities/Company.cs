using System;
using System.Collections.Generic;

namespace ManagementMusicPlayer.DAL.Entities;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<MusicPlayer> MusicPlayers { get; set; } = new List<MusicPlayer>();
}
