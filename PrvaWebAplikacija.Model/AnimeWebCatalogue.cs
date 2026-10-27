namespace PrvaWebAplikacija.Model
{
    public class AnimeWebCatalogue
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int NumberOfSeasons { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public Guid StudioId { get; set; }
        public Guid? MangaId { get; set; }
        public string? Popularity { get; set; }
        public string? StudioName { get; set; }
        public string? MangaTitle { get; set; }
        public string? Author { get; set; }
    }
}
