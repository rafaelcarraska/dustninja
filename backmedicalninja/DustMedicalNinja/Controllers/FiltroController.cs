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
    public class FiltroController : ControllerBase
    {

        [Authorize(Roles = "Filtros_Visualizar")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Filtro> Lista()
        {
            return new FiltroBusiness(HttpContext).ListAll();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaCombo()
        {
            return new FiltroBusiness(HttpContext).ListCombo();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaComboStatus()
        {
            return new FiltroBusiness(HttpContext).ListaComboStatus();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaComboDatas()
        {
            return new FiltroBusiness(HttpContext).ListaComboDatas();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaComboGerais()
        {
            return new FiltroBusiness(HttpContext).ListaComboGerais();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaComboOrdem()
        {
            return new FiltroBusiness(HttpContext).ListaComboOrdem();
        }

        [Authorize(Roles = "Filtros_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public Filtro ListaFiltro(string Id)
        {
            return new FiltroBusiness(HttpContext).List(Id);
        }

        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] Filtro filtro)
        {
            SegurancaBusiness segurancaBusiness = new SegurancaBusiness(HttpContext);

            if (string.IsNullOrEmpty(filtro.Id) &&
                 !segurancaBusiness.Verifica_Acesso("Filtros_Adicionar"))
            {
                return Unauthorized();
            }
            if (!string.IsNullOrEmpty(filtro.Id) &&
                !segurancaBusiness.Verifica_Acesso("Filtros_Editar"))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new FiltroBusiness(HttpContext).Salvar(filtro));
        }

        [Authorize(Roles = "Filtros_Deletar")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Deleta(string Id)
        {
            return Ok(new FiltroBusiness(HttpContext).Delete(Id));
        }
    }
}
