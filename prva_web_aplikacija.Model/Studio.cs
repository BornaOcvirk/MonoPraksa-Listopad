using System;
using System.Collections.Generic;

namespace prva_web_aplikacija.Model;

public partial class Studio
{
    public Guid StudioId { get; set; }

    public string NameS { get; set; } = null!;

    public string? Country { get; set; }

    public DateOnly? Established { get; set; }

    public virtual ICollection<Anime> Animes { get; set; } = new List<Anime>();

    public virtual ICollection<Manga> Mangas { get; set; } = new List<Manga>();
}
