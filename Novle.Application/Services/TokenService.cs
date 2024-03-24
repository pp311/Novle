using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Novle.Application.Common.Configurations;
using Novle.Domain.Entities;
using Novle.Domain.Repositories.Base;

namespace Novle.Application.Services
{
    public class TokenService(IOptionsMonitor<TokenSettings> tokenSettings,
                              IUnitOfWork unitOfWork,
                              IMapper mapper) : BaseService(unitOfWork, mapper)
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = tokenSettings.CurrentValue.Key;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenSettings.CurrentValue.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public (string, DateTime) GenerateRefreshToken()
            => (Guid.NewGuid().ToString().Replace("-", ""), DateTime.UtcNow.AddHours(tokenSettings.CurrentValue.RefreshTokenExpiryInHours));
    }
}
