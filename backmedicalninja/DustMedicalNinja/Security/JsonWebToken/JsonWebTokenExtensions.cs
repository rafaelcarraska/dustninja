using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace DustMedicalNinja.Security
{
    public static class JsonWebTokenExtensions
    {
        public static void AddJti(this ICollection<Claim> claims)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        }

        public static void AddRoles(this ICollection<Claim> claims, List<string> roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }

        public static void AddSub(this ICollection<Claim> claims, string sub)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, sub));
        }

        public static void AddCompany(this ICollection<Claim> claims, string company)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sid, company));
        }

        public static void AddUsuarioId(this ICollection<Claim> claims, string usuarioId)
        {
            claims.Add(new Claim(ClaimTypes.GivenName, usuarioId));
        }

        public static void AddMaster(this ICollection<Claim> claims, bool master)
        {
            claims.Add(new Claim(ClaimTypes.GroupSid, master.ToString()));
        }
    }
}
