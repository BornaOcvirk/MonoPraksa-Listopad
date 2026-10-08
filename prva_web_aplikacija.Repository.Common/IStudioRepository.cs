using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Repository.Common
{
    public interface IStudioRepository
    {
        Task<List<Studio>> GetAllAsync();
        Task<Studio?> GetByIdAsync(Guid id);
    }
}