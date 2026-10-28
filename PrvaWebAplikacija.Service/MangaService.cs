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
                throw new NotFoundException("Manga not found.");
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

        public async Task<Manga> AddAsync(CreateMangaDto dto)
        {
            var manga = MangaRules.Normalize(dto, dto.StudioId);

            if (!await _repository.StudioExistsAsync(dto.StudioId))
                throw new BusinessRuleException("Studio doesn't exist.");

            if (await _repository.MangaExistsAsync(manga.Title, manga.Author))
                throw new BusinessRuleException("This manga already exists.");

            return await _repository.PostAsync(manga)
                ?? throw new BusinessRuleException("The manga could not be saved.");
        }
    }
}
