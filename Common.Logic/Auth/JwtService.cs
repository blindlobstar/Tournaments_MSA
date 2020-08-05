using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Common.Core.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Common.Logic.Auth
{
    public class JwtService : IJwtService
    {
        private readonly IJwtOptions _jwtOptions;
        private readonly SigningCredentials _signingCredentials;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtService(IJwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            _signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);
        }

        public IJwtToken CreateToken(string userId, string role, Dictionary<string, string> claims = null)
        {
            var payload = new List<Claim>{
                new Claim(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.Role, role)
            };

            var customClaims = claims?.Select(claim => new Claim(claim.Key, claim.Value)).ToArray() ??
                Array.Empty<Claim>();

            payload.AddRange(customClaims);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: payload,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.Lifetime),
                signingCredentials: _signingCredentials
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JwtToken()
            {
                AccessToken = accessToken,
                RefreshToken = string.Empty
            };
        }
    }
}