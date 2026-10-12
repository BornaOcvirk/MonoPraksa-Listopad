using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Model;

namespace PrvaWebAplikacija.Repository.Common
{
    public interface IAnimeWebShopRepository
    {
        Task<List<AnimeWebShop>> GetAllAsync();
        Task<AnimeWebShop?> GetByIdAsync(Guid id);
        Task<List<AnimeWebShop>> GetByGenreAsync(string genre, int minSeasons);
        Task<List<AnimeWebShop>> GetByAuthorAsync(string author);
        Task AddAsync(AnimeWebShop anime);
        Task<bool> UpdateAsync(Guid id, AnimeWebShop anime);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> StudioExistsAsync(Guid studioId);
    }
}

