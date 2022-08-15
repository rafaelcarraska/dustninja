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
    public class TipoExameController : ControllerBase
    {

        [Authorize(Roles = "TipoExame_Visualizar")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<TipoExame> Lista()
        {
            return new TipoExameBusiness(HttpContext).ListAll();
        }

        [Authorize(Roles = "TipoExame_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public TipoExame ListaTipoExame(string Id)
        {
            return new TipoExameBusiness(HttpContext).List(Id);
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaCombo()
        {
            return new TipoExameBusiness(HttpContext).ListCombo();
        }

        [HttpGet("/[controller]/[action]/{modalidade}")]
        public IEnumerable<Combobox> listaTipoEstudo(string modalidade)
        {
            return new TipoExameBusiness(HttpContext).ListaTipoEstudo(modalidade);
        }

        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] TipoExame tipoExame)
        {
            SegurancaBusiness segurancaBusiness = new SegurancaBusiness(HttpContext);

            if (string.IsNullOrEmpty(tipoExame.Id) &&
                 !segurancaBusiness.Verifica_Acesso("TipoExame_Adicionar"))
            {
                return Unauthorized();
            }
            if (!string.IsNullOrEmpty(tipoExame.Id) &&
                !segurancaBusiness.Verifica_Acesso("TipoExame_Editar"))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new TipoExameBusiness(HttpContext).Salvar(tipoExame));
        }

        [Authorize(Roles = "TipoExame_Deletar")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Deleta(string Id)
        {
            return Ok(new TipoExameBusiness(HttpContext).Delete(Id));
        }
    }
}
