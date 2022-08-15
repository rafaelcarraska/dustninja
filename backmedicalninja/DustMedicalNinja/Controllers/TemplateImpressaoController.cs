using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DustMedicalNinja.Business;
using DustMedicalNinja.Models;
using Microsoft.AspNetCore.Authorization;
using DustMedicalNinja.Models.ViewModel;

namespace DustMedicalNinja.Controllers
{
    [ApiController]
    public class TemplateImpressaoController : ControllerBase
    {

        [Authorize(Roles = "TemplateImpressao_Visualizar")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<TemplateImpressao> Lista()
        {
            return new TemplateImpressaoBusiness(HttpContext).ListAll();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaCombo()
        {
            return new TemplateImpressaoBusiness(HttpContext).ListCombo();
        }

        [HttpGet("/[controller]/[action]/{Id}")]
        public IEnumerable<Combobox> ListaComboByFacility(string Id)
        {
            return new TemplateImpressaoBusiness(HttpContext).ListaComboByFacility(Id);
        }

        [Authorize(Roles = "TemplateImpressao_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public TemplateImpressaoViewModel ListaTemplateImpressao(string Id)
        {
            return new TemplateImpressaoBusiness(HttpContext).ListViewModel(Id);
        }

        [HttpGet("/[controller]/[action]/{Id}")]
        public Msg TemplateImpresasaoUtilizado(string Id)
        {
            return new Msg()
            {
                id = new TemplateImpressaoBusiness(HttpContext).TemplateImpresasaoUtilizado(Id)
            };
        }

        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] TemplateImpressao templateImpressao)
        {
            SegurancaBusiness segurancaBusiness = new SegurancaBusiness(HttpContext);

            if (string.IsNullOrEmpty(templateImpressao.Id) &&
                 !segurancaBusiness.Verifica_Acesso("TemplateImpressao_Adicionar"))
            {
                return Unauthorized();
            }
            if (!string.IsNullOrEmpty(templateImpressao.Id) &&
                !segurancaBusiness.Verifica_Acesso("TemplateImpressao_Editar"))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new TemplateImpressaoBusiness(HttpContext).Salvar(templateImpressao));
        }

        [Authorize(Roles = "TemplateImpressao_Deletar")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Deleta(string Id)
        {
            return Ok(new TemplateImpressaoBusiness(HttpContext).Delete(Id));
        }

        [Authorize(Roles = "TemplateImpressao_Visualizar")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<AnexoTemplate> listaAnexos()
        {
            return new AnexoTemplatesBusiness(HttpContext).List();
        }

        [Authorize(Roles = "TemplateImpressao_Editar")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> DeletarAnexo(string Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new AnexoTemplatesBusiness(HttpContext).Delete(Id));
        }
    }
}
