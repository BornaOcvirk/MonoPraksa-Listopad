using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Repository.Common
{
    public interface IStudioRepository
    {
        Task<List<Studio>> GetAllAsync();
        Task<Studio?> GetStudioByIdAsync(Guid id);
        Task<Studio?> PostAsync(Studio studio);
        Task<Studio?> PutAsync(Guid id, Studio studio);
        Task<Studio?> DeleteAsync(Guid id);
    }
}