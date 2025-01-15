using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Core.Entities;

namespace WebApplication1.Service
{
    public class TokenService
    {
        private readonly IConfiguration configuration;
        public TokenService(IConfiguration configuration) { 

        this.configuration = configuration;
        }
        public async Task<string> GenerateTokenAsync(User user)
        {
            var AuthClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, user.name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:issuer"],
                audience: configuration["JWT:audience"],
                claims: AuthClaims,
                signingCredentials: creds,
                expires: DateTime.Now.AddDays(30)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
