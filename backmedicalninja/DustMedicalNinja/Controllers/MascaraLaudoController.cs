using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DustMedicalNinja.Business;
using DustMedicalNinja.Models;
using Microsoft.AspNetCore.Authorization;

namespace DustMedicalNinja.Controllers
{
    [ApiController]
    public class MascaraLaudoController : ControllerBase
    {

        [Authorize(Roles = "MascaraLaudo_Visualizar")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<MascaraLaudo> Lista()
        {
            return new MascaraLaudoBusiness(HttpContext).ListAll();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaCombo()
        {
            return new MascaraLaudoBusiness(HttpContext).ListCombo();
        }

        [Authorize(Roles = "MascaraLaudo_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public MascaraLaudo ListaMascaraLaudo(string Id)
        {
            return new MascaraLaudoBusiness(HttpContext).List(Id);
        }

        [HttpGet("/[controller]/[action]/{Id}")]
        public IEnumerable<MascaraLaudo> listaOrderbyModalidade(string Id)
        {
            return new MascaraLaudoBusiness(HttpContext).ListaOrderbyModalidade(Id);
        }
        
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] MascaraLaudo mascaraLaudo)
        {
            SegurancaBusiness segurancaBusiness = new SegurancaBusiness(HttpContext);

            if (string.IsNullOrEmpty(mascaraLaudo.Id) &&
                 !segurancaBusiness.Verifica_Acesso("MascaraLaudo_Adicionar"))
            {
                return Unauthorized();
            }
            if (!string.IsNullOrEmpty(mascaraLaudo.Id) &&
                !segurancaBusiness.Verifica_Acesso("MascaraLaudo_Editar"))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new MascaraLaudoBusiness(HttpContext).Salvar(mascaraLaudo));
        }

        [Authorize(Roles = "MascaraLaudo_Deletar")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Deleta(string Id)
        {
            return Ok(new MascaraLaudoBusiness(HttpContext).Delete(Id));
        }
    }
}
