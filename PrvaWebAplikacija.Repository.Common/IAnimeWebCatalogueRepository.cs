using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Model;

namespace PrvaWebAplikacija.Repository.Common
{
    public interface IAnimeWebCatalogueRepository
    {
        Task<List<AnimeWebCatalogue>> GetAllAsync();
        Task<AnimeWebCatalogue?> GetByIdAsync(Guid id);
        Task<List<AnimeWebCatalogue>> GetByGenreAsync(string genre, int minSeasons);
        Task<List<AnimeWebCatalogue>> GetByAuthorAsync(string author);
        Task AddAsync(AnimeWebCatalogue anime);
        Task<bool> UpdateAsync(Guid id, AnimeWebCatalogue anime);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> StudioExistsAsync(Guid studioId);
        Task<bool> MangaExistsAsync(Guid mangaId);
        Task<PagedResult<AnimeWebCatalogue>> SearchAsync(AnimeCatalogueQuery query);
        Task<List<string>> GetGenresAsync();
    }
}

