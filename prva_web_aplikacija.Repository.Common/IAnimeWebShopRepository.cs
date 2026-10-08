using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Repository.Common
{
    public interface IAnimeWebShopRepository
    {
        List<AnimeWebShop> GetAll();
        AnimeWebShop? GetById(Guid id);
        List<AnimeWebShop> GetByGenre(string genre, int minSeasons);
        List<AnimeWebShop> GetByAuthor(string author);
        void Add(AnimeWebShop anime);
        bool Update(Guid id, AnimeWebShop anime);
        bool Delete(Guid id);
        bool StudioExists(Guid studioId);
    }
}

