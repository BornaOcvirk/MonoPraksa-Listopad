using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Studio?> GetStudioByIdAsync(Guid id)
        {
            return await _context.Studios.FirstOrDefaultAsync(s => s.StudioId == id);
        }

        public async Task<Studio?> PostAsync(Studio studio)
        {
            _context.Studios.Add(studio);
            await _context.SaveChangesAsync();
            return studio;
        }

        public async Task<Studio?> PutAsync(Guid id, Studio studio)
        {
            var existingStudio = await GetStudioByIdAsync(id);
            if (existingStudio == null)
                return null;

            studio.StudioId = id;
            _context.Entry(existingStudio).CurrentValues.SetValues(studio);
            await _context.SaveChangesAsync();
            return existingStudio;
        }

        public async Task<Studio?> DeleteAsync(Guid id)
        {
            var studio = await GetStudioByIdAsync(id);
            if (studio == null)
                return null;

            _context.Studios.Remove(studio);
            await _context.SaveChangesAsync();
            return studio;
        }
    }
}