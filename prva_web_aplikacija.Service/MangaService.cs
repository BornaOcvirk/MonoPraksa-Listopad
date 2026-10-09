using System;
using System.Collections.Generic;
using System.Text;

using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Service
{
    public class MangaService : IMangaService
    {
        private readonly IMangaRepository _repository;

        public MangaService(IMangaRepository repository)
        {
            _repository = repository;
        }
        public Task<List<Manga>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Manga?> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<Manga>> GetByAuthorAsync(string author)
        {
            return _repository.GetByAuthorAsync(author);
        }

        public Task<List<Manga>> GetByStudioAsync(Guid studioId)
        {
            return _repository.GetByStudioAsync(studioId);
        }
    }
}
