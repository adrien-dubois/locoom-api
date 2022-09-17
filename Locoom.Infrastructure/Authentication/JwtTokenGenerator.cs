using Locoom.Application.Common.Interfaces.Authentication;
using Locoom.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Locoom.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IDatetimeProvider _datetimeProvider;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IDatetimeProvider datetimeProvider, IOptions<JwtSettings> jwtOptions)
        {
            _datetimeProvider = datetimeProvider;
            _jwtSettings = jwtOptions.Value;
        }

        public string GeneratorToken(Guid userId, string firstName, string lastName)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256
                );
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, firstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString())
            };

            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: _datetimeProvider.UtcNow.AddDays(_jwtSettings.ExpiryDays),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
