using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Model;
namespace PrvaWebAplikacija.Repository.Common
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);

        Task<bool> DoesUserExist(string username);
        Task<User> AddAsync(User user);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateRoleAsync(Guid id, string role);
    }
}
