using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TimeZoneManager.Authentication
{
    internal class JwtTokenGenerator : IJwtTokenGenerator
    {
        private const int DefaultMinutesTimeout = 120;

        public string GenerateToken(JwtTokenPayload payload, string secret)
        {
            return GenerateToken(payload, secret, DefaultMinutesTimeout);
        }

        public string GenerateToken(JwtTokenPayload payload, string secret, int minutesTimeout)
        {
            byte[] symmetricKey = Encoding.UTF8.GetBytes(secret);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            List<Claim> listClaims = new List<Claim>
            {
                new Claim(CustomClaimTypes.Name, payload.Username),
            };

            foreach (string permission in payload.Permissions)
            {
                listClaims.Add(new Claim(CustomClaimTypes.Permissions, permission));
            }

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(listClaims),
                Expires = DateTime.UtcNow.AddMinutes(minutesTimeout),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken stoken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
