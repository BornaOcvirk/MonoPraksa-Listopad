using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;
using PrvaWebAplikacija.Service.Common;
using PrvaWebAplikacija.Common;

namespace PrvaWebAplikacija.Service
{
    public class StudioService : IStudioService
    {
        private readonly IStudioRepository _studioRepository;

        public StudioService(IStudioRepository studioRepository)
        {
            _studioRepository = studioRepository;
        }

        public async Task<List<Studio>> GetAllAsync()
        {
            return await _studioRepository.GetAllAsync();
        }

        public async Task<Studio> GetStudioByIdAsync(Guid id)
        {
            var studio = await _studioRepository.GetStudioByIdAsync(id);
            if (studio == null)
                throw new NotFoundException("Studio ne postoji.");
            return studio;
        }

        public async Task<Studio?> PostAsync(Studio studio)
        {
            return await _studioRepository.PostAsync(studio);
        }

        public async Task<Studio?> PutAsync(Guid id, Studio studio)
        {
            return await _studioRepository.PutAsync(id, studio);
        }

        public async Task<Studio?> DeleteAsync(Guid id)
        {
            return await _studioRepository.DeleteAsync(id);
        }
    }
}