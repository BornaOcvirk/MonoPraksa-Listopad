using System;
using System.Collections.Generic;

namespace prva_web_aplikacija.Model;

public partial class Manga
{
    public Guid MangaId { get; set; }

    public string Author { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int Volumes { get; set; }

    public string? Popularity { get; set; }

    public Guid StudioId { get; set; }

    public virtual ICollection<Anime> Animes { get; set; } = new List<Anime>();

    public virtual Studio Studio { get; set; } = null!;
}
