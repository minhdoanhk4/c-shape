using System;
using System.Collections.Generic;

namespace ManagementMusicPlayer.DAL.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<MusicPlayer> MusicPlayers { get; set; } = new List<MusicPlayer>();
}
