using System;
using System.Collections.Generic;
using System.Text;

using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Service
{
    public class StudioService : IStudioService
    {
        private readonly IStudioRepository _repository;

        public StudioService(IStudioRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Studio>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Studio?> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }
    }
}
