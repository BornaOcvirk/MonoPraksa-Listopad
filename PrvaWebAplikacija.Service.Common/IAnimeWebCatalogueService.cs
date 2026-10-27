using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Model;

namespace PrvaWebAplikacija.Service.Common
{
    public interface IAnimeWebCatalogueService
    {
        Guid InstanceId { get; }
        Task<List<AnimeWebCatalogue>> GetAllAsync();
        Task<AnimeWebCatalogue> GetByIdAsync(Guid id);
        Task<List<AnimeWebCatalogue>> GetByGenreAsync(string genre, int numberOfSeasons);
        Task<List<AnimeWebCatalogue>> GetByAuthorAsync(string author);
        Task<AnimeWebCatalogue> AddAsync(AnimeWebCatalogue anime);
        Task<bool> UpdateAsync(Guid id, AnimeWebCatalogue anime);
        Task<bool> DeleteAsync(Guid id);
        Task<PagedResult<AnimeWebCatalogue>> SearchAsync(AnimeCatalogueQuery query);
        Task<List<string>> GetGenresAsync();
    }
}
