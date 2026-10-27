namespace PrvaWebAplikacija.Model
{
    // Body for POST api/Manga. Strings are nullable so missing values reach the
    // service validation (with readable messages) instead of the automatic 400.
    public class CreateMangaDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int Volumes { get; set; }
        public string? Popularity { get; set; }
        public Guid StudioId { get; set; }
    }
}
