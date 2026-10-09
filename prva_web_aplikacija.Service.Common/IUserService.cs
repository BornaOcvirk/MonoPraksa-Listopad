using System;
using System.Collections.Generic;
using System.Text;
using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Service.Common
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> CreateAsync(CreateUserDto createUserDto);
        Task<UserDto?> LoginAsync(UserLoginDto userLoginDto);
        Task<UserDto?> DeleteAsync(Guid id);
    }
}
