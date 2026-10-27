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
        private static readonly string[] PopularityOptions = { "Very high", "High", "Medium", "Low" };

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

        public async Task<Manga> AddAsync(CreateMangaDto dto)
        {
            var title = dto.Title?.Trim();
            var author = dto.Author?.Trim();
            var popularity = string.IsNullOrWhiteSpace(dto.Popularity) ? null : dto.Popularity.Trim();

            if (string.IsNullOrEmpty(title))
                throw new BusinessRuleException("Manga mora imati naslov.");

            if (string.IsNullOrEmpty(author))
                throw new BusinessRuleException("Manga mora imati autora.");

            if (title.Length > 255 || author.Length > 255)
                throw new BusinessRuleException("Naslov i autor mogu imati najviše 255 znakova.");

            if (dto.Volumes < 0)
                throw new BusinessRuleException("Broj svezaka ne može biti negativan.");

            if (popularity != null && !PopularityOptions.Contains(popularity))
                throw new BusinessRuleException("Neispravna popularnost.");

            if (!await _repository.StudioExistsAsync(dto.StudioId))
                throw new BusinessRuleException("Studio ne postoji.");

            var manga = new Manga
            {
                Title = title,
                Author = author,
                Volumes = dto.Volumes,
                Popularity = popularity,
                StudioId = dto.StudioId,
            };

            return await _repository.PostAsync(manga)
                ?? throw new BusinessRuleException("Manga nije spremljena.");
        }
    }
}
