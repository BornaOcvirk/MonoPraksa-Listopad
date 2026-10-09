using System;
using System.Collections.Generic;
using System.Text;

using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Service.Common
{
    public interface IStudioService
    {
        Task<List<Studio>> GetAllAsync();
        Task<Studio?> GetByIdAsync(Guid id);

        Task<List<Studio>> PutAsync(Guid id, Studio studio);

        Task<Studio?> PostAsync(Studio studio);

        Task<Studio?> DeleteAsync(Guid id);
    }
}
