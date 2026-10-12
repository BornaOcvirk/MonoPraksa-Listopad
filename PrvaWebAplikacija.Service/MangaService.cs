using System;
using System.Collections.Generic;
using System.Text;

using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;
using PrvaWebAplikacija.Service.Common;
using PrvaWebAplikacija.Common;

namespace PrvaWebAplikacija.Service
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

        public async Task<Manga> GetByIdAsync(Guid id)
        {
            var manga = await _repository.GetByIdAsync(id);
            if (manga == null)
                throw new NotFoundException("Manga ne postoji.");
            return manga;
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
