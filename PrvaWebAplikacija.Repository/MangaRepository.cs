using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;

namespace PrvaWebAplikacija.Repository
{
    public class MangaRepository : IMangaRepository
    {
        private readonly PraksaDbContext _context;

        public MangaRepository(PraksaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Manga>> GetAllAsync()
        {
            return await _context.Mangas
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        public async Task<Manga?> GetByIdAsync(Guid id)
        {
            return await _context.Mangas.FindAsync(id);
        }

        public async Task<Manga?> PutAsync(Guid id, Manga manga)
        {
            var existingManga = await _context.Mangas.FindAsync(id);
            if(existingManga == null)
            {
                return null;
            }
            _context.Mangas.Update(manga);
            await _context.SaveChangesAsync();
            return manga;
        }

        public async Task<Manga?> PostAsync(Manga manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
            return manga;
        }

        public async Task<Manga?> DeleteAsync(Guid id)
        {
            var manga = await _context.Mangas.FindAsync(id);
            if (manga == null)
            {
                return null;
            }
            _context.Mangas.Remove(manga);
            await _context.SaveChangesAsync();
            return manga;
        }

        public async Task<List<Manga>> GetByAuthorAsync(string author)
        {
            var pattern = LikePattern.Contains(author.Trim());
            return await _context.Mangas
                .AsNoTracking()
                .Where(m => EF.Functions.ILike(m.Author, pattern, LikePattern.EscapeChar))
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        public async Task<List<Manga>> GetByStudioAsync(Guid studioId)
        {
            return await _context.Mangas
                .AsNoTracking()
                .Where(m => m.StudioId == studioId)
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        public async Task<bool> StudioExistsAsync(Guid studioId)
        {
            return await _context.Studios.AnyAsync(s => s.StudioId == studioId);
        }

        // Same title and author, ignoring case
        public async Task<bool> MangaExistsAsync(string title, string author)
        {
            var t = title.ToLower();
            var a = author.ToLower();
            return await _context.Mangas.AnyAsync(m => m.Title.ToLower() == t && m.Author.ToLower() == a);
        }
    }
}
