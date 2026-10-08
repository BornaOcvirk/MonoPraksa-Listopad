namespace prva_web_aplikacija.Model
{
    public class AnimeWebShop
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int NumberOfSeasons { get; set; }
        public decimal Price { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public Guid StudioId { get; set; }
    }
}
