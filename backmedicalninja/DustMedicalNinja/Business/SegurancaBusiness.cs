using DustMedicalNinja.Extensions;
using DustMedicalNinja.Models;
using DustMedicalNinja.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DustMedicalNinja.Business
{
    internal class SegurancaBusiness
    {
        private readonly HttpContext _HttpContext;
        private string usuarioId;

        internal SegurancaBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal bool Verifica_Acesso(string permissao)
        {
            if (_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Where(c => c.Value == permissao).Count() > 0) 
            { 
                return true;
            }
            return false;
        }
    }
}
