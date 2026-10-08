using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Service.Common
{
    public interface IAnimeWebShopService
    {
        Guid InstanceId { get; }
        List<AnimeWebShop> GetAll();
        AnimeWebShop? GetById(Guid id);
        List<AnimeWebShop> GetByGenre(string genre, int numberOfSeasons);
        List<AnimeWebShop> GetByAuthor(string author);
        AnimeWebShop Add(AnimeWebShop anime);
        bool Update(Guid id, AnimeWebShop anime);
        bool Delete(Guid id);
        decimal? GetFinalPrice(Guid id);
    }
}
