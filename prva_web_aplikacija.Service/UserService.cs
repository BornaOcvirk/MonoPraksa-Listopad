using Microsoft.AspNetCore.Identity;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Repository.Common;
using prva_web_aplikacija.Service.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace prva_web_aplikacija.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(ToDto).ToList();
        }
        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : ToDto(user);
        }

        public async Task<UserDto?> CreateAsync(CreateUserDto dto)
        {
            var username = dto.Username.Trim();

            if (await _userRepository.DoesUserExist(username))
                return null;

            var user = new User
            {
                Username = username,
                Role = "Customer"
            };
            user.Password_hash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddAsync(user);
            return ToDto(user);
        }

        public async Task<UserDto?> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username.Trim());
            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password_hash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return ToDto(user);
        }
        public async Task<UserDto?> DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            var deleted = await _userRepository.DeleteAsync(id);
            return deleted ? ToDto(user) : null;
        }

        private static UserDto ToDto(User user) => new UserDto
        {
            User_id = user.User_id,
            Username = user.Username,
            Role = user.Role
        };
    }
}