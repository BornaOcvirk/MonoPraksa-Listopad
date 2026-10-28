using System;
using System.Collections.Generic;
using System.Text;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PrvaWebAplikacija.Repository
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
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.Username)
                .ToListAsync();
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.User_id == id);
        }
        // Usernames are compared ignoring case, so "Borna" and "borna" are the same account.
        // ToLower instead of ILike, because "_" in a username would act as a wildcard.
        public async Task<User?> GetByUsernameAsync(string username)
        {
            var name = username.ToLower();
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username.ToLower() == name);
        }
        public async Task<bool> DoesUserExist(string username)
        {
            var name = username.ToLower();
            return await _context.Users.AnyAsync(u => u.Username.ToLower() == name);
        }
        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        // A single DELETE statement: if the row is already gone it returns false instead of throwing
        public async Task<bool> DeleteAsync(Guid id)
        {
            var rows = await _context.Users.Where(u => u.User_id == id).ExecuteDeleteAsync();
            return rows > 0;
        }
        public async Task<bool> UpdateRoleAsync(Guid id, string role)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Role = role;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
