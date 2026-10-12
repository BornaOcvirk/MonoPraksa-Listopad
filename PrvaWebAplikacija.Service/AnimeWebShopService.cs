using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Common;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;
using PrvaWebAplikacija.Service.Common;

namespace PrvaWebAplikacija.Service
{
    public class AnimeWebShopService : IAnimeWebShopService
    {
        private readonly IAnimeWebShopRepository _repository;
        private readonly IIdGenerator _idGenerator;
        private readonly IPriceCalculator _priceCalculator;

        public Guid InstanceId { get; } = Guid.NewGuid();

        public AnimeWebShopService(
            IAnimeWebShopRepository repository,
            IIdGenerator idGenerator,
            IPriceCalculator priceCalculator)
        {
            _repository = repository;
            _idGenerator = idGenerator;
            _priceCalculator = priceCalculator;
        }

        public async Task<List<AnimeWebShop>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<AnimeWebShop?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<AnimeWebShop>> GetByGenreAsync(string genre, int numberOfSeasons)
        {
            return await _repository.GetByGenreAsync(genre, numberOfSeasons);
        }

        public async Task<List<AnimeWebShop>> GetByAuthorAsync(string author)
        {
            return await _repository.GetByAuthorAsync(author);
        }

        public async Task<AnimeWebShop> AddAsync(AnimeWebShop anime)
        {
            await ValidateAsync(anime);
            anime.Id = _idGenerator.NextId();
            await _repository.AddAsync(anime);
            return anime;
        }

        public async Task<bool> UpdateAsync(Guid id, AnimeWebShop anime)
        {
            await ValidateAsync(anime);
            return await _repository.UpdateAsync(id, anime);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<decimal?> GetFinalPriceAsync(Guid id)
        {
            var anime = await _repository.GetByIdAsync(id);
            if (anime == null)
                return null;

            return _priceCalculator.CalculateFinalPrice(anime.Price);
        }

        private async Task ValidateAsync(AnimeWebShop anime)
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