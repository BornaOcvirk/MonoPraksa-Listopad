using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;

namespace PrvaWebAplikacija.Repository
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

        public async Task<List<AnimeWebShop>> GetAllAsync()
        {
            return await _context.Animes
                .OrderBy(a => a.Title)
                .Select(ToModel)
                .ToListAsync();
        }

        public async Task<AnimeWebShop?> GetByIdAsync(Guid id)
        {
            return await _context.Animes
                .Where(a => a.AnimeId == id)
                .Select(ToModel)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AnimeWebShop>> GetByGenreAsync(string genre, int minSeasons)
        {
            return await _context.Animes
                .Where(a => a.Genre == genre && a.NumberOfSeasons >= minSeasons)
                .OrderBy(a => a.Title)
                .Select(ToModel)
                .ToListAsync();
        }

        public async Task<List<AnimeWebShop>> GetByAuthorAsync(string author)
        {
            return await _context.Animes
                .Where(a => a.Manga != null && EF.Functions.ILike(a.Manga.Author, "%" + author + "%"))
                .OrderBy(a => a.Title)
                .Select(ToModel)
                .ToListAsync();
        }

        public async Task AddAsync(AnimeWebShop anime)
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

            await _context.Animes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, AnimeWebShop anime)
        {
            var entity = await _context.Animes.FirstOrDefaultAsync(a => a.AnimeId == id);
            if (entity == null)
                return false;

            entity.Title = anime.Name;
            entity.Genre = anime.Genre;
            entity.NumberOfSeasons = anime.NumberOfSeasons;
            entity.Price = anime.Price;
            entity.ReleseDate = anime.ReleaseDate;
            entity.StudioId = anime.StudioId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Animes.FirstOrDefaultAsync(a => a.AnimeId == id);
            if (entity == null)
                return false;

            _context.Animes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StudioExistsAsync(Guid studioId)
        {
            return await _context.Studios.AnyAsync(s => s.StudioId == studioId);
        }
    }
}