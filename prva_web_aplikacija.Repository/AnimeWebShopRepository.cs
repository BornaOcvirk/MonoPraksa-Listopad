using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;

namespace prva_web_aplikacija.Repository
{
    public class AnimeWebShopRepository : IAnimeWebShopRepository
    {
        private readonly PraksaDbContext _context;

        public AnimeWebShopRepository(PraksaDbContext context)
        {
            _context = context;
        }
        private static readonly Expression<Func<Anime, AnimeWebShop>> ToModel = a => new AnimeWebShop
        {
            Id = a.AnimeId,
            Name = a.Title,
            Genre = a.Genre ?? string.Empty,
            NumberOfSeasons = a.NumberOfSeasons ?? 0,
            Price = a.Price ?? 0m,
            ReleaseDate = a.ReleseDate,
            StudioId = a.StudioId
        };

        public List<AnimeWebShop> GetAll()
        {
            return _context.Animes
                .OrderBy(a => a.Title)
                .Select(ToModel)
                .ToList();
        }

        public AnimeWebShop? GetById(Guid id)
        {
            return _context.Animes
                .Where(a => a.AnimeId == id)
                .Select(ToModel)
                .FirstOrDefault();
        }

        public List<AnimeWebShop> GetByGenre(string genre, int minSeasons)
        {
            return _context.Animes
                .Where(a => a.Genre == genre && a.NumberOfSeasons >= minSeasons)
                .OrderBy(a => a.Title)
                .Select(ToModel)
                .ToList();
        }

        public List<AnimeWebShop> GetByAuthor(string author)
        {
            return _context.Animes
                .Where(a => a.Manga != null && EF.Functions.ILike(a.Manga.Author, "%" + author + "%"))
                .OrderBy(a => a.Title)
                .Select(ToModel)
                .ToList();
        }

        public void Add(AnimeWebShop anime)
        {
            var entity = new Anime
            {
                AnimeId = anime.Id,
                Title = anime.Name,
                Genre = anime.Genre,
                NumberOfSeasons = anime.NumberOfSeasons,
                Price = anime.Price,
                ReleseDate = anime.ReleaseDate,
                StudioId = anime.StudioId
            };

            _context.Animes.Add(entity);
            _context.SaveChanges();
        }

        public bool Update(Guid id, AnimeWebShop anime)
        {
            var entity = _context.Animes.FirstOrDefault(a => a.AnimeId == id);
            if (entity == null)
                return false;

            entity.Title = anime.Name;
            entity.Genre = anime.Genre;
            entity.NumberOfSeasons = anime.NumberOfSeasons;
            entity.Price = anime.Price;
            entity.ReleseDate = anime.ReleaseDate;
            entity.StudioId = anime.StudioId;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(Guid id)
        {
            var entity = _context.Animes.FirstOrDefault(a => a.AnimeId == id);
            if (entity == null)
                return false;

            _context.Animes.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public bool StudioExists(Guid studioId)
        {
            return _context.Studios.Any(s => s.StudioId == studioId);
        }
    }
}