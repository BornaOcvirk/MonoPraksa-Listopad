using System;
using System.Collections.Generic;
using System.Text;

using PrvaWebAplikacija.Model;

namespace PrvaWebAplikacija.Service.Common
{
    public interface IMangaService
    {
        Task<List<Manga>> GetAllAsync();
        Task<Manga> GetByIdAsync(Guid id);
        Task<List<Manga>> GetByAuthorAsync(string author);
        Task<List<Manga>> GetByStudioAsync(Guid studioId);
    }
}
