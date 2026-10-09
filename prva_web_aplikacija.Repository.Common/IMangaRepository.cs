using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Repository.Common
{
    public interface IMangaRepository
    {
        Task<List<Manga>> GetAllAsync();
        Task<Manga?> GetByIdAsync(Guid id);

        Task<Manga?> PutAsync(Guid id, Manga manga);

        Task<Manga?> PostAsync(Manga manga);

        Task<Manga?> DeleteAsync(Guid id);
        Task<List<Manga>> GetByAuthorAsync(string author);
        Task<List<Manga>> GetByStudioAsync(Guid studioId);
    }
}
