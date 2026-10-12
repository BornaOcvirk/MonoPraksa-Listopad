using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Model;

namespace PrvaWebAplikacija.Service.Common
{
    public interface IAnimeWebShopService
    {
        Guid InstanceId { get; }
        Task<List<AnimeWebShop>> GetAllAsync();
        Task<AnimeWebShop?> GetByIdAsync(Guid id);
        Task<List<AnimeWebShop>> GetByGenreAsync(string genre, int numberOfSeasons);
        Task<List<AnimeWebShop>> GetByAuthorAsync(string author);
        Task<AnimeWebShop> AddAsync(AnimeWebShop anime);
        Task<bool> UpdateAsync(Guid id, AnimeWebShop anime);
        Task<bool> DeleteAsync(Guid id);
        Task<decimal?> GetFinalPriceAsync(Guid id);
    }
}
