using PrvaWebAplikacija.Common;
using PrvaWebAplikacija.Model;

namespace PrvaWebAplikacija.Service
{
    // Checks a new manga and turns it into an entity. Used when adding a manga on its own
    // and when it is added together with an anime.
    internal static class MangaRules
    {
        private const int MaxTextLength = 255; // varchar(255) in the database

        public static Manga Normalize(CreateMangaDto dto, Guid studioId)
        {
            var title = dto.Title?.Trim();
            var author = dto.Author?.Trim();
            var popularity = string.IsNullOrWhiteSpace(dto.Popularity) ? null : dto.Popularity.Trim();

            if (string.IsNullOrEmpty(title))
                throw new BusinessRuleException("Manga must have a title.");

            if (string.IsNullOrEmpty(author))
                throw new BusinessRuleException("Manga must have an author.");

            if (title.Length > MaxTextLength || author.Length > MaxTextLength)
                throw new BusinessRuleException($"Manga title and author can be at most {MaxTextLength} characters.");

            if (dto.Volumes < 0)
                throw new BusinessRuleException("Number of volumes can't be negative.");

            if (!Popularity.IsValid(popularity))
                throw new BusinessRuleException($"Popularity must be one of: {string.Join(", ", Popularity.Options)}.");

            return new Manga
            {
                Title = title,
                Author = author,
                Volumes = dto.Volumes,
                Popularity = popularity,
                StudioId = studioId,
            };
        }
    }
}
