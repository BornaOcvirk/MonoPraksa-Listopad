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
        AnimeWebShop? GetById(int id);
        List<AnimeWebShop> GetByGenre(string genre, int numberOfSeasons);
        AnimeWebShop Add(AnimeWebShop anime);
        bool Update(int id, AnimeWebShop anime);
        bool Delete(int id);
        float? GetFinalPrice(int id);
    }
}
