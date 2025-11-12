using System;
using System.Collections.Generic;

namespace ManagementMusicPlayer.DAL.Entities;

public partial class MusicPlayer
{
    public int Id { get; set; }

    public string PlayerName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public DateOnly PublishDate { get; set; }

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;
}
