namespace PrvaWebAplikacija.Model
{
    public class AnimeCatalogueQuery
    {
        public string? Search { get; set; }     
        public string? Genre { get; set; }
        public string? Author { get; set; }      
        public Guid? StudioId { get; set; }
        public Guid? MangaId { get; set; }
        public int? MinSeasons { get; set; }
        public int? Year { get; set; }
        public string? Popularity { get; set; }

        public string? SortBy { get; set; }     
        public bool SortDescending { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}