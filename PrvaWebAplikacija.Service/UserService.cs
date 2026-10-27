using Microsoft.AspNetCore.Identity;
using PrvaWebAplikacija.Common;
using PrvaWebAplikacija.Model;
using PrvaWebAplikacija.Repository.Common;
using PrvaWebAplikacija.Service.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace PrvaWebAplikacija.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private const int MinPasswordLength = 8;

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
        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("Korisnik ne postoji.");
            return ToDto(user);
        }

        public async Task<UserDto?> CreateAsync(CreateUserDto dto)
        {
            // Only checked on register, so existing users with shorter passwords can still log in
            if (string.IsNullOrEmpty(dto.Password) || dto.Password.Length < MinPasswordLength)
                throw new BusinessRuleException($"Password must be at least {MinPasswordLength} characters.");

            var username = dto.Username.Trim();

            if (await _userRepository.DoesUserExist(username))
                return null;

            var user = new User
            {
                Username = username,
                Role = Roles.Customer
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

        public async Task<UserDto> MakeAdminAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("Korisnik ne postoji.");

            if (user.Role == Roles.Admin)
                throw new BusinessRuleException("Korisnik je već administrator.");

            if (!await _userRepository.UpdateRoleAsync(id, Roles.Admin))
                throw new NotFoundException("Korisnik ne postoji.");

            user.Role = Roles.Admin;
            return ToDto(user);
        }

        private static UserDto ToDto(User user) => new UserDto
        {
            User_id = user.User_id,
            Username = user.Username,
            Role = user.Role
        };
    }
}