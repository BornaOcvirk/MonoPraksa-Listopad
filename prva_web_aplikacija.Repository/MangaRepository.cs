using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;

namespace prva_web_aplikacija.Repository
{
    public class MangaRepository : IMangaRepository
    {
        private readonly PraksaDbContext _context;

        public MangaRepository(PraksaDbContext context)
        {
            _context = context;
        }

        public List<Manga> GetAll()
        {
            return _context.Mangas
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToList();
        }

        public Manga? GetById(Guid id)
        {
            return _context.Mangas
                .AsNoTracking()
                .FirstOrDefault(m => m.MangaId == id);
        }

        public List<Manga> GetByAuthor(string author)
        {
            return _context.Mangas
                .AsNoTracking()
                .Where(m => EF.Functions.ILike(m.Author, "%" + author + "%"))
                .OrderBy(m => m.Title)
                .ToList();
        }

        public List<Manga> GetByStudio(Guid studioId)
        {
            return _context.Mangas
                .AsNoTracking()
                .Where(m => m.StudioId == studioId)
                .OrderBy(m => m.Title)
                .ToList();
        }
    }
}
