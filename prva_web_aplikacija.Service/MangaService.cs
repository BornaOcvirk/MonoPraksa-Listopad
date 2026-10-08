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

        public List<Manga> GetAll()
        {
            return _repository.GetAll();
        }

        public Manga? GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public List<Manga> GetByAuthor(string author)
        {
            return _repository.GetByAuthor(author);
        }

        public List<Manga> GetByStudio(Guid studioId)
        {
            return _repository.GetByStudio(studioId);
        }
    }
}
