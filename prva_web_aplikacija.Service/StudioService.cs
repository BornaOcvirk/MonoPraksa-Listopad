using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Service
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

        public async Task<Studio?> GetStudioByIdAsync(Guid id)
        {
            return await _studioRepository.GetStudioByIdAsync(id);
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