using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Common;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Service
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

        public List<AnimeWebShop> GetAll()
        {
            return _repository.GetAll();
        }

        public AnimeWebShop? GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public List<AnimeWebShop> GetByGenre(string genre, int numberOfSeasons)
        {
            return _repository.GetByGenre(genre, numberOfSeasons);
        }

        public List<AnimeWebShop> GetByAuthor(string author)
        {
            return _repository.GetByAuthor(author);
        }

        public AnimeWebShop Add(AnimeWebShop anime)
        {
            Validate(anime);
            anime.Id = _idGenerator.NextId();
            _repository.Add(anime);
            return anime;
        }

        public bool Update(Guid id, AnimeWebShop anime)
        {
            Validate(anime);
            return _repository.Update(id, anime);
        }

        public bool Delete(Guid id)
        {
            return _repository.Delete(id);
        }

        public decimal? GetFinalPrice(Guid id)
        {
            var anime = _repository.GetById(id);
            if (anime == null)
                return null;

            return _priceCalculator.CalculateFinalPrice(anime.Price);
        }

        private void Validate(AnimeWebShop anime)
        {
            if (string.IsNullOrWhiteSpace(anime.Name))
                throw new BuisnessExeptions("Anime mora imati naziv.");

            if (anime.Price <= 0)
                throw new BuisnessExeptions("Cijena mora biti veća od 0.");

            if (anime.NumberOfSeasons < 0)
                throw new BuisnessExeptions("Broj sezona ne može biti negativan.");

            if (anime.ReleaseDate == default)
                throw new BuisnessExeptions("Anime mora imati datum izlaska.");

            if (!_repository.StudioExists(anime.StudioId))
                throw new BuisnessExeptions("Studio ne postoji.");
        }
    }
}