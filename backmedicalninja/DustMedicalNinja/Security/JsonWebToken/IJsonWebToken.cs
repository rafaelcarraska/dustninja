using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace DustMedicalNinja.Security
{
    public interface IJsonWebToken
    {
        TokenValidationParameters TokenValidationParameters { get; }

        Dictionary<string, object> Decode(string token);

        string Encode(string sub, List<string> roles, string company, string usuarioId, bool master);
    }
}
