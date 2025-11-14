using System;
using System.Collections.Generic;

namespace ManagementMusicPlayer.DAL.Entities;

public partial class MusicPlayer
{
    public int PlayerId { get; set; }

    public string PlayerName { get; set; } = null!;

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public DateTime PublishDate { get; set; }

    public int CompanyId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;
}
