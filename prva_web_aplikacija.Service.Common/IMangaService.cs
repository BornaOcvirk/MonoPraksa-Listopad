using System;
using System.Collections.Generic;
using System.Text;

using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Service.Common
{
    public interface IMangaService
    {
        Task<List<Manga>> GetAllAsync();
        Task<Manga?> GetByIdAsync(Guid id);
        Task<List<Manga>> GetByAuthorAsync(string author);
        Task<List<Manga>> GetByStudioAsync(Guid studioId);
    }
}
