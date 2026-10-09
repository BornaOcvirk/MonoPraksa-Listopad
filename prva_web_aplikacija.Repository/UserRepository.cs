using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace prva_web_aplikacija.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PraksaDbContext _context;

        public UserRepository(PraksaDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync()
                .asNoTracking()
                .OrderBy(u => u.Username)
                .ToListAsync();
        }
        Task<User?> GetByIdAsync(Guid id)
        {

        }
        Task<User?> GetByUsernameAsync(string username)
        {

        }
        Task<bool> DoesUserExist(string username)
        {

        }
        Task<User> AddAsync(User user)
        {

        }
        Task<bool> DeleteAsync(Guid id)
        {

        }
    }
}
