using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Common;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;
using PrvaWebAplikacija.Service.Common;

namespace PrvaWebAplikacija.Service
{
    public class AnimeWebCatalogueService : IAnimeWebCatalogueService
    {
        // Shared by the release date check and the year filter
        private const int MinYear = 1900;
        private const int MaxYear = 2100;
        private const int MaxNameLength = 200;
        private const int MaxGenreLength = 100; // varchar(100) in the database
        private const int MaxSeasons = 100;
        private const int MaxPage = 100_000;    // stops (Page - 1) * PageSize from overflowing

        private readonly IAnimeWebCatalogueRepository _repository;
        private readonly IMangaRepository _mangaRepository;
        private readonly IIdGenerator _idGenerator;

        public Guid InstanceId { get; } = Guid.NewGuid();

        public AnimeWebCatalogueService(
            IAnimeWebCatalogueRepository repository,
            IMangaRepository mangaRepository,
            IIdGenerator idGenerator)
        {
            _repository = repository;
            _mangaRepository = mangaRepository;
            _idGenerator = idGenerator;
        }

        public async Task<List<AnimeWebCatalogue>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<AnimeWebCatalogue> GetByIdAsync(Guid id)
        {
            var anime = await _repository.GetByIdAsync(id);
            if (anime == null)
                throw new NotFoundException("Anime not found.");
            return anime;
        }

        public async Task<List<AnimeWebCatalogue>> GetByGenreAsync(string genre, int numberOfSeasons)
        {
            return await _repository.GetByGenreAsync(genre.Trim(), numberOfSeasons);
        }

        public async Task<List<AnimeWebCatalogue>> GetByAuthorAsync(string author)
        {
            return await _repository.GetByAuthorAsync(author.Trim());
        }

        public async Task<AnimeWebCatalogue> AddAsync(AnimeWebCatalogue anime)
        {
            var newManga = await ValidateAsync(anime, null);
            anime.Id = _idGenerator.NextId();
            await _repository.AddAsync(anime, newManga);
            anime.NewManga = null;
            return anime;
        }

        public async Task<bool> UpdateAsync(Guid id, AnimeWebCatalogue anime)
        {
            var existingAnime = await _repository.GetByIdAsync(id);
            if (existingAnime == null)
                throw new NotFoundException("Anime not found.");

            var newManga = await ValidateAsync(anime, id);
            if (!await _repository.UpdateAsync(id, anime, newManga))
                throw new NotFoundException("Anime not found."); // deleted while we were validating
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            // One DELETE statement, so two deletes at once give 404 instead of a crash
            if (!await _repository.DeleteAsync(id))
                throw new NotFoundException("Anime not found.");
            return true;
        }

        public async Task<PagedResult<AnimeWebCatalogue>> SearchAsync(AnimeCatalogueQuery query)
        {
            // Keep paging within safe limits
            query.Page = Math.Clamp(query.Page, 1, MaxPage);
            query.PageSize = Math.Clamp(query.PageSize, 1, 100);
            query.Search = query.Search?.Trim();
            query.Author = query.Author?.Trim();
            query.Genre = query.Genre?.Trim();
            query.Popularity = query.Popularity?.Trim();

            if (query.Year is < MinYear or > MaxYear)
                throw new BusinessRuleException($"Year must be between {MinYear} and {MaxYear}.");

            return await _repository.SearchAsync(query);
        }

        public async Task<List<string>> GetGenresAsync()
        {
            return await _repository.GetGenresAsync();
        }

        // Cleans up and checks the anime. Returns the new manga to save with it, if one was sent.
        private async Task<Manga?> ValidateAsync(AnimeWebCatalogue anime, Guid? id)
        {
            anime.Name = anime.Name?.Trim() ?? string.Empty;
            anime.Genre = anime.Genre?.Trim() ?? string.Empty;
            anime.Popularity = string.IsNullOrWhiteSpace(anime.Popularity) ? null : anime.Popularity.Trim();

            if (anime.Name.Length == 0)
                throw new BusinessRuleException("Anime must have a title.");

            if (anime.Name.Length > MaxNameLength)
                throw new BusinessRuleException($"Title can be at most {MaxNameLength} characters.");

            if (anime.Genre.Length == 0)
                throw new BusinessRuleException("Anime must have a genre.");

            if (anime.Genre.Length > MaxGenreLength)
                throw new BusinessRuleException($"Genre can be at most {MaxGenreLength} characters.");

            if (anime.NumberOfSeasons is < 0 or > MaxSeasons)
                throw new BusinessRuleException($"Number of seasons must be between 0 and {MaxSeasons}.");

            if (anime.ReleaseDate == default)
                throw new BusinessRuleException("Anime must have a release date.");

            if (anime.ReleaseDate.Year is < MinYear or > MaxYear)
                throw new BusinessRuleException($"Release date must be between {MinYear} and {MaxYear}.");

            if (!Popularity.IsValid(anime.Popularity))
                throw new BusinessRuleException($"Popularity must be one of: {string.Join(", ", Popularity.Options)}.");

            if (!await _repository.StudioExistsAsync(anime.StudioId))
                throw new BusinessRuleException("Studio doesn't exist.");

            if (await _repository.AnimeExistsAsync(anime.Name, anime.ReleaseDate, id))
                throw new BusinessRuleException("An anime with this title and release date already exists.");

            // A new manga is saved in the same transaction as the anime, so a failed save leaves nothing behind
            if (anime.NewManga != null)
            {
                var manga = MangaRules.Normalize(anime.NewManga, anime.StudioId);
                if (await _mangaRepository.MangaExistsAsync(manga.Title, manga.Author))
                    throw new BusinessRuleException("This manga already exists. Pick it from the list instead.");
                anime.MangaId = null;
                return manga;
            }

            if (anime.MangaId.HasValue && !await _repository.MangaExistsAsync(anime.MangaId.Value))
                throw new BusinessRuleException("Manga doesn't exist.");

            return null;
        }
    }
}
