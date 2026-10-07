using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;


namespace prva_web_aplikacija.Repository
{
    public class AnimeWebShopRepository : IAnimeWebShopRepository
    {
        private static readonly List<AnimeWebShop> animelist = new List<AnimeWebShop>
        {
            new AnimeWebShop(1, "Naruto", "Shounen", 2, 19.99f),
            new AnimeWebShop(2, "One Piece", "Adventure", 5, 29.99f),
            new AnimeWebShop(3, "Attack on Titan", "Action", 4, 24.99f),
            new AnimeWebShop(4, "My Hero Academia", "Superhero", 3, 14.99f),
            new AnimeWebShop(5, "Demon Slayer", "Action", 2, 19.99f),
        };

        public List<AnimeWebShop> GetAll()
        {
            return animelist;
        }

        public AnimeWebShop? GetById(int id)
        {
            for (int i = 0; i < animelist.Count; i++)
            {
                if (animelist[i].Id == id)
                {
                    return animelist[i];
                }
            }
            return null;
        }

        public void Add(AnimeWebShop anime)
        {
            animelist.Add(anime);
        }

        public bool Update(int id, AnimeWebShop anime) 
        {
            for (int i = 0; i < animelist.Count; i++)
            {
                if (animelist[i].Id == id)
                {
                    animelist[i].Name = anime.Name;
                    animelist[i].Genre = anime.Genre;
                    animelist[i].NumberOfSeasons = anime.NumberOfSeasons;
                    animelist[i].Price = anime.Price;
                    return true;
                }
            }
            return false;
        }

        public bool Delete(int id)
        {
            for (int i = 0; i < animelist.Count; i++)
            {
                if (animelist[i].Id == id)
                {
                    animelist.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }

    
}
