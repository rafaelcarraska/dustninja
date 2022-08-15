using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustMedicalNinja.Models;
using System.Security.Claims;

namespace DustMedicalNinja.Policies
{
    public class OnlyExpensiveMastreAuthorizationHandler : AuthorizationHandler<OnlyExpensiveMastreRequirement, Usuario>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyExpensiveMastreRequirement requirement, Usuario resource)
        {
            var UsuarioId  = context.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();

            if (resource != null && resource.Id.ToString() == UsuarioId.ToString())
            {
                context.Succeed(requirement);                
            }
            return Task.CompletedTask;
        }
    }
}
