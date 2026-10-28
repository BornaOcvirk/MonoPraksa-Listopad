using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;

namespace PrvaWebAplikacija.Repository
{
    public class AnimeWebCatalogueRepository : IAnimeWebCatalogueRepository
    {
        private readonly PraksaDbContext _context;

        public AnimeWebCatalogueRepository(PraksaDbContext context)
        {
            _context = context;
        }
        private static readonly Expression<Func<Anime, AnimeWebCatalogue>> ToModel = a => new AnimeWebCatalogue
        {
            Id = a.AnimeId,
            Name = a.Title,
            Genre = a.Genre ?? string.Empty,
            NumberOfSeasons = a.NumberOfSeasons ?? 0,
            ReleaseDate = a.ReleseDate,
            StudioId = a.StudioId,
            MangaId = a.MangaId,
            Popularity = a.Popularity,
            StudioName = a.Studio.NameS,
            MangaTitle = a.Manga != null ? a.Manga.Title : null,
            Author = a.Manga != null ? a.Manga.Author : null
        };

        // Popularity is stored as text, so turn it into a number before sorting.
        // Otherwise "High" would come before "Very high" alphabetically.
        private static readonly Expression<Func<Anime, int>> PopularityRank = a =>
            a.Popularity == "Very high" ? 4 :
            a.Popularity == "High" ? 3 :
            a.Popularity == "Medium" ? 2 :
            a.Popularity == "Low" ? 1 : 0;

        public async Task<List<AnimeWebCatalogue>> GetAllAsync()
        {
            return await _context.Animes
                .OrderBy(a => a.Title)
                .Select(ToModel)
                .ToListAsync();
        }

        public async Task<AnimeWebCatalogue?> GetByIdAsync(Guid id)
        {
            return await _context.Animes
                .Where(a => a.AnimeId == id)
                .Select(ToModel)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AnimeWebCatalogue>> GetByGenreAsync(string genre, int minSeasons)
        {
            var pattern = LikePattern.Escape(genre);
            return await _context.Animes
                .Where(a => a.Genre != null && EF.Functions.ILike(a.Genre, pattern, LikePattern.EscapeChar) && a.NumberOfSeasons >= minSeasons)
                .OrderBy(a => a.Title)
                .Select(ToModel)
                .ToListAsync();
        }

        public async Task<List<AnimeWebCatalogue>> GetByAuthorAsync(string author)
        {
            var pattern = LikePattern.Contains(author);
            return await _context.Animes
                .Where(a => a.Manga != null && EF.Functions.ILike(a.Manga.Author, pattern, LikePattern.EscapeChar))
                .OrderBy(a => a.Title)
                .Select(ToModel)
                .ToListAsync();
        }

        // newManga (optional) is inserted in the same SaveChanges, so both rows are saved or neither is
        public async Task AddAsync(AnimeWebCatalogue anime, Manga? newManga = null)
        {
            var entity = new Anime
            {
                AnimeId = anime.Id,
                Title = anime.Name,
                Genre = anime.Genre,
                NumberOfSeasons = anime.NumberOfSeasons,
                ReleseDate = anime.ReleaseDate,
                StudioId = anime.StudioId,
                MangaId = anime.MangaId,
                Popularity = anime.Popularity,
                Manga = newManga
            };

            await _context.Animes.AddAsync(entity);
            await _context.SaveChangesAsync();
            anime.MangaId = entity.MangaId;
        }

        public async Task<bool> UpdateAsync(Guid id, AnimeWebCatalogue anime, Manga? newManga = null)
        {
            var entity = await _context.Animes.FirstOrDefaultAsync(a => a.AnimeId == id);
            if (entity == null)
                return false;

            entity.Title = anime.Name;
            entity.Genre = anime.Genre;
            entity.NumberOfSeasons = anime.NumberOfSeasons;
            entity.ReleseDate = anime.ReleaseDate;
            entity.StudioId = anime.StudioId;
            entity.Popularity = anime.Popularity;
            if (newManga != null)
                entity.Manga = newManga;
            else
                entity.MangaId = anime.MangaId;

            await _context.SaveChangesAsync();
            anime.MangaId = entity.MangaId;
            return true;
        }

        // A single DELETE statement: if the row is already gone it returns false instead of throwing
        public async Task<bool> DeleteAsync(Guid id)
        {
            var rows = await _context.Animes.Where(a => a.AnimeId == id).ExecuteDeleteAsync();
            return rows > 0;
        }

        // Same title (ignoring case) and release date, optionally skipping the anime being edited
        public async Task<bool> AnimeExistsAsync(string title, DateOnly releaseDate, Guid? excludeId)
        {
            var t = title.ToLower();
            return await _context.Animes.AnyAsync(a =>
                a.Title.ToLower() == t && a.ReleseDate == releaseDate && (excludeId == null || a.AnimeId != excludeId));
        }

        public async Task<bool> StudioExistsAsync(Guid studioId)
        {
            return await _context.Studios.AnyAsync(s => s.StudioId == studioId);
        }

        public async Task<bool> MangaExistsAsync(Guid mangaId)
        {
            return await _context.Mangas.AnyAsync(m => m.MangaId == mangaId);
        }

        public async Task<PagedResult<AnimeWebCatalogue>> SearchAsync(AnimeCatalogueQuery query)
        {
            var animes = _context.Animes.AsQueryable();

            // Filters: each one is only applied if it was sent.
            // Text is escaped so "%" and "_" typed by the user don't act as wildcards.
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = LikePattern.Contains(query.Search);
                animes = animes.Where(a => EF.Functions.ILike(a.Title, search, LikePattern.EscapeChar));
            }

            if (!string.IsNullOrWhiteSpace(query.Genre))
            {
                var genre = LikePattern.Escape(query.Genre);
                animes = animes.Where(a => a.Genre != null && EF.Functions.ILike(a.Genre, genre, LikePattern.EscapeChar));
            }

            if (!string.IsNullOrWhiteSpace(query.Author))
            {
                var author = LikePattern.Contains(query.Author);
                animes = animes.Where(a => a.Manga != null && EF.Functions.ILike(a.Manga.Author, author, LikePattern.EscapeChar));
            }

            if (query.StudioId.HasValue)
                animes = animes.Where(a => a.StudioId == query.StudioId.Value);

            if (query.MangaId.HasValue)
                animes = animes.Where(a => a.MangaId == query.MangaId.Value);

            if (query.MinSeasons.HasValue)
                animes = animes.Where(a => a.NumberOfSeasons >= query.MinSeasons.Value);

            if (query.Year.HasValue)
                animes = animes.Where(a => a.ReleseDate.Year == query.Year.Value);

            if (!string.IsNullOrWhiteSpace(query.Popularity))
            {
                var popularity = LikePattern.Escape(query.Popularity);
                animes = animes.Where(a => a.Popularity != null && EF.Functions.ILike(a.Popularity, popularity, LikePattern.EscapeChar));
            }

            // Count before paging so the frontend knows how many pages there are
            var totalCount = await animes.CountAsync();

            IOrderedQueryable<Anime> sorted = (query.SortBy?.ToLower(), query.SortDescending) switch
            {
                ("releasedate", false) => animes.OrderBy(a => a.ReleseDate),
                ("releasedate", true)  => animes.OrderByDescending(a => a.ReleseDate),
                ("seasons", false)     => animes.OrderBy(a => a.NumberOfSeasons),
                ("seasons", true)      => animes.OrderByDescending(a => a.NumberOfSeasons),
                ("popularity", false)  => animes.OrderBy(PopularityRank),
                ("popularity", true)   => animes.OrderByDescending(PopularityRank),
                (_, true)              => animes.OrderByDescending(a => a.Title),
                _                      => animes.OrderBy(a => a.Title)
            };

            var items = await sorted
                .ThenBy(a => a.Title)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(ToModel)
                .ToListAsync();

            return new PagedResult<AnimeWebCatalogue>
            {
                Items = items,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<List<string>> GetGenresAsync()
        {
            return await _context.Animes
                .Where(a => a.Genre != null && a.Genre != "")
                .Select(a => a.Genre!)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();
        }
    }
}