using System;
using System.Collections.Generic;

namespace prva_web_aplikacija.Model;

public partial class Anime
{
    public Guid AnimeId { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly ReleseDate { get; set; }

    public string? Popularity { get; set; }

    public Guid? MangaId { get; set; }

    public Guid StudioId { get; set; }

    public string? Genre { get; set; }

    public int? NumberOfSeasons { get; set; }

    public decimal? Price { get; set; }

    public virtual Manga? Manga { get; set; }

    public virtual Studio Studio { get; set; } = null!;
}
