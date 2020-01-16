using TimeZoneManager.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace TimeZoneManager.Authentication
{
    internal class JwtManager : IJwtManager
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly string _secret = Guid.Empty.ToString();

        public JwtManager(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            // TODO: Get secret from DB and pass as argument in constructor
        }

        public string GenerateToken(JwtTokenPayload payload)
        {
            return _jwtTokenGenerator.GenerateToken(payload, _secret);
        }

        public string GenerateToken(JwtTokenPayload payload, int minutesTimeout)
        {
            return _jwtTokenGenerator.GenerateToken(payload, _secret, minutesTimeout);
        }

        public JwtTokenPayload GetPayload(string authHeader)
        {
            ClaimsPrincipal principal = GetPrincipal(authHeader);

            return new JwtTokenPayload()
            {
                Username = GetClaim(principal, CustomClaimTypes.Name),
                Permissions = GetClaims(principal, CustomClaimTypes.Permissions)
            };
        }

        public string GetTokenFromAuthorizationHeader(string authHeader)
        {
            string tokenPrefix = $"{JwtConstants.Bearer} ";
            if (authHeader.IndexOf(tokenPrefix) == 0)
            {
                return authHeader.Substring(tokenPrefix.Length);
            }
            return string.Empty;
        }

        private ClaimsPrincipal GetPrincipal(string authHeader)
        {
            try
            {
                string token = GetTokenFromAuthorizationHeader(authHeader);
                var tokenHandler = new JwtSecurityTokenHandler();

                if (!(tokenHandler.ReadToken(token) is JwtSecurityToken jwtToken))
                {
                    return null;
                }

                var symmetricKey = Encoding.UTF8.GetBytes(_secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
                return principal;
            }
            catch (Exception e)
            {
                throw new InvalidTokenException(e.Message, e);
            }
        }

        private static string GetClaim(ClaimsPrincipal principal, string type)
        {
            return principal.Claims.Where(c => c.Type == type).Select(c => c.Value).FirstOrDefault();
        }

        private static IEnumerable<string> GetClaims(ClaimsPrincipal principal, string type)
        {
            return principal.Claims.Where(c => c.Type == type).Select(c => c.Value);
        }
    }
}
