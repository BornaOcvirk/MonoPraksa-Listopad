using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Common;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;
using PrvaWebAplikacija.Service.Common;

namespace PrvaWebAplikacija.Service
{
    public class AnimeWebCatalogueService : IAnimeWebCatalogueService
    {
        private readonly IAnimeWebCatalogueRepository _repository;
        private readonly IIdGenerator _idGenerator;
        private readonly IPriceCalculator _priceCalculator;

        public Guid InstanceId { get; } = Guid.NewGuid();

        public AnimeWebCatalogueService(
            IAnimeWebCatalogueRepository repository,
            IIdGenerator idGenerator,
            IPriceCalculator priceCalculator)
        {
            _repository = repository;
            _idGenerator = idGenerator;
            _priceCalculator = priceCalculator;
        }

        public async Task<List<AnimeWebCatalogue>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<AnimeWebCatalogue> GetByIdAsync(Guid id)
        {
            var anime = await _repository.GetByIdAsync(id);
            if (anime == null)
                throw new NotFoundException("Anime ne postoji.");
            return anime;
        }

        public async Task<List<AnimeWebCatalogue>> GetByGenreAsync(string genre, int numberOfSeasons)
        {
            return await _repository.GetByGenreAsync(genre, numberOfSeasons);
        }

        public async Task<List<AnimeWebCatalogue>> GetByAuthorAsync(string author)
        {
            return await _repository.GetByAuthorAsync(author);
        }

        public async Task<AnimeWebCatalogue> AddAsync(AnimeWebCatalogue anime)
        {
            await ValidateAsync(anime);
            anime.Id = _idGenerator.NextId();
            await _repository.AddAsync(anime);
            return anime;
        }

        public async Task<bool> UpdateAsync(Guid id, AnimeWebCatalogue anime)
        {
            var existingAnime = await _repository.GetByIdAsync(id);
            if (existingAnime == null)
                throw new NotFoundException("Anime ne postoji.");
            await ValidateAsync(anime);
            return await _repository.UpdateAsync(id, anime);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingAnime = await _repository.GetByIdAsync(id);
            if (existingAnime == null)
                throw new NotFoundException("Anime ne postoji.");

            return await _repository.DeleteAsync(id);
        }

        public async Task<decimal> GetFinalPriceAsync(Guid id)
        {
            var anime = await _repository.GetByIdAsync(id);
            if (anime == null)
                throw new NotFoundException("Anime ne postoji.");

            return _priceCalculator.CalculateFinalPrice(anime.Price);
        }

        private async Task ValidateAsync(AnimeWebCatalogue anime)
        {

            if (string.IsNullOrWhiteSpace(anime.Name))
                throw new BusinessRuleException("Anime mora imati naziv.");

            if (anime.Price <= 0)
                throw new BusinessRuleException("Cijena mora biti veća od 0.");

            if (anime.NumberOfSeasons < 0)
                throw new BusinessRuleException("Broj sezona ne može biti negativan.");

            if (anime.ReleaseDate == default)
                throw new BusinessRuleException("Anime mora imati datum izlaska.");

            if (!await _repository.StudioExistsAsync(anime.StudioId))
                throw new BusinessRuleException("Studio ne postoji.");
        }

        
    }
}