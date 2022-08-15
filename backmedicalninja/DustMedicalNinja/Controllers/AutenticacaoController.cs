using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DustMedicalNinja.Models;
using DustMedicalNinja.Business;
using DustMedicalNinja.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;

namespace DustMedicalNinja.Controllers
{
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public AutenticacaoController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [AllowAnonymous]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> SelecionaEmpresa([FromBody] Autenticacao autenticacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new AutenticacaoBusiness(HttpContext).SelecionaEmpresa(autenticacao));
        }

        [AllowAnonymous]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Autentica([FromBody] Autenticacao autenticacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new AutenticacaoBusiness(HttpContext).validaLogin(autenticacao));
        }

        [AllowAnonymous]
        [HttpGet("/[controller]/[action]")]
       
        public async Task<IActionResult> Logoff()
        {
            //await HttpContext.SignOutAsync("Cookie");
            HttpContext.Session.Clear();
            return Ok("Logoff");
        }

        [HttpGet("/[controller]/[action]")]

        public async Task<IActionResult> DadosBasicos()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new AutenticacaoBusiness(HttpContext).DadosBasicos());
        }
    }
}