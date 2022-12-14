using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DustMedicalNinja.Security
{
    public class JsonWebToken : IJsonWebToken
    {
        public TokenValidationParameters TokenValidationParameters => new TokenValidationParameters
        {
            IssuerSigningKey = JsonWebTokenSettings.SecurityKey,
            ValidateActor = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidAudience = JsonWebTokenSettings.Audience,
            ValidIssuer = JsonWebTokenSettings.Issuer
        };

        public Dictionary<string, object> Decode(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token).Payload;
        }

        public string Encode(string sub, List<string> roles, string company, string usuarioId, bool master)
        {
            var claims = new List<Claim>();
            claims.AddJti();
            claims.AddRoles(roles);
            claims.AddSub(sub);
            claims.AddCompany(company);
            claims.AddUsuarioId(usuarioId);
            claims.AddMaster(master);

            return new JwtSecurityTokenHandler().WriteToken(CreateJwtSecurityToken(claims));
        }

        private static JwtSecurityToken CreateJwtSecurityToken(IEnumerable<Claim> claims)
        {
            return new JwtSecurityToken
            (
                JsonWebTokenSettings.Issuer,
                JsonWebTokenSettings.Audience,
                claims,
                DateTime.UtcNow,
                JsonWebTokenSettings.Expires,
                JsonWebTokenSettings.SigningCredentials
            );
        }
    }
}
