using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using prva_web_aplikacija.Model;

namespace prva_web_aplikacija.Auth
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _settings;

        public TokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public AuthResponseDto CreateToken(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var expires = DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("sub", user.User_id.ToString()),
                    new Claim("name", user.Username),
                    new Claim("role", user.Role)
                }),
                Expires = expires,
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var token = new JsonWebTokenHandler().CreateToken(descriptor);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expires,
                User = user
            };
        }
    }
}
