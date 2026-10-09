using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Model;
namespace prva_web_aplikacija.Repository.Common
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);

        Task<bool> DoesUserExist(string username);
        Task<User> AddAsync(User user);
        Task<bool> DeleteAsync(Guid id);
    }
}
