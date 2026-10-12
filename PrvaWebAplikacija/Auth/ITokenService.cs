using PrvaWebAplikacija.Model;

namespace PrvaWebAplikacija.Auth
{
    public interface ITokenService
    {
        AuthResponseDto CreateToken(UserDto user);
    }
}