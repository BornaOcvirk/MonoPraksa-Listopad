using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Model;

namespace PrvaWebAplikacija.Repository.Common
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
        Task<bool> StudioExistsAsync(Guid studioId);
        Task<bool> MangaExistsAsync(string title, string author);
    }
}
