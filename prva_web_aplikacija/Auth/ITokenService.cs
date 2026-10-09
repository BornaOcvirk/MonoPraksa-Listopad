using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Auth
{
    public interface ITokenService
    {
        AuthResponseDto CreateToken(UserDto user);
    }
}