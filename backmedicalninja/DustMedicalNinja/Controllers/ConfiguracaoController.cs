using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DustMedicalNinja.Business;
using DustMedicalNinja.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Security.Claims;

namespace DustMedicalNinja.Controllers
{
    [ApiController]
    public class ConfiguracaoController : ControllerBase
    {
        Msg msg = new Msg();
        private readonly IAuthorizationService authorizationService;

        public ConfiguracaoController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpGet("/[controller]/[action]")]
        public Configuracao listaConfiguracao()
        {
            return new ConfiguracaoBusiness(HttpContext).List();
        }
    
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] Configuracao configuracao)
        {  
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new ConfiguracaoBusiness(HttpContext).Salvar(configuracao));
        }
    }
}
