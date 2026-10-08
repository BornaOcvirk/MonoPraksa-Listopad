using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;

namespace prva_web_aplikacija.Repository
{
    public class StudioRepository : IStudioRepository
    {
        private readonly PraksaDbContext _context;

        public StudioRepository(PraksaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Studio>> GetAllAsync()
        {
            return await _context.Studios
                .AsNoTracking()
                .OrderBy(s => s.NameS)
                .ToListAsync();
        }

        public async Task<Studio?> GetByIdAsync(Guid id)
        {
            return await _context.Studios
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudioId == id);
        }

        public async Task<List<Studio>> PutByAuthorAsync(Studio studio)
        
        {
            _context.Studios.Update(studio);
            await _context.SaveChangesAsync();
            return new List<Studio> {studio};
        }
        public async Task<Studio?> PostAsync(Guid id, Studio studio)
        {
            _context.Studios.Add(studio);
            await _context.SaveChangesAsync();
            return studio;
        }

        public async Task<Studio?> DeleteAsync(Guid id)
        {
            var studio = await GetByIdAsync(id);
            if (studio == null)
                return null;

            _context.Studios.Remove(studio);
            await _context.SaveChangesAsync();
            return studio;
        }

    }
}

