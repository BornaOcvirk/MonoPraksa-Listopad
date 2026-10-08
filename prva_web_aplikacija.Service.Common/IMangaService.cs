using System;
using System.Collections.Generic;
using System.Text;

using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Service.Common
{
    public interface IMangaService
    {
        List<Manga> GetAll();
        Manga? GetById(Guid id);
        List<Manga> GetByAuthor(string author);
        List<Manga> GetByStudio(Guid studioId);
    }
}
