using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace WebApplication1.Repositry
    {
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;
        using global::WebApplication1.Core.Entities;
        using global::WebApplication1.Repositry.Data;

        namespace WebApplication1.Core.services
        {
            public class AuthService
            {
                private readonly PlantCareContext _context;
                private readonly TokenService _jwtTokenService;

                public AuthService(PlantCareContext context, TokenService jwtTokenService)
                {
                    _context = context;
                    _jwtTokenService = jwtTokenService;
                }

                public async Task<string> RegisterOrLogin(User user)
                {
                    // Generate JWT
                    var tokenValue = await _jwtTokenService.GenerateTokenAsync(user);

                    // Invalidate previous tokens
                    var oldTokens = _context.Tokens.Where(t => t.userId == user.Id);
                    foreach (var oldToken in oldTokens)
                    {
                        oldToken.isValid = false;
                    }

                    // Save new token
                    var newToken = new Token
                    {
                        token = tokenValue,
                        isValid = true,
                        userId = user.Id
                    };
                    _context.Tokens.Add(newToken);

                    await _context.SaveChangesAsync();

                    return tokenValue;
                }
            }

        }

    }

}
