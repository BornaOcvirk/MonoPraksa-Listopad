using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Repository.Common
{
    public interface IAnimeWebShopRepository
    {
        List<AnimeWebShop> GetAll();
        AnimeWebShop? GetById(int id);
        void Add(AnimeWebShop anime);
        bool Update(int id, AnimeWebShop anime);
        bool Delete(int id);
    }
}

