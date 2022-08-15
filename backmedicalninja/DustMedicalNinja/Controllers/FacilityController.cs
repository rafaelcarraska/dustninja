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
    public class FacilityController : ControllerBase
    {
        [Authorize(Roles = "Facility_Visualizar")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Facility> Lista()
        {           
            return new FacilityBusiness(HttpContext).ListAll();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaCombo()
        {
            return new FacilityBusiness(HttpContext).ListaCombo();
        }

        [HttpGet("/[controller]/[action]/{id}")]
        public IEnumerable<Combobox> ListaComboByUsuario(string id)
        {
            return new FacilityBusiness(HttpContext).ListaComboByUsuario(id);
        }

        [Authorize(Roles = "Facility_Visualizar")]
        [HttpGet("/[controller]/[action]/{id}")]
        public IEnumerable<Permissao> ListaPermissao(string id)
        {
            return new PermissaoBusiness(HttpContext).List_FacilityId(id);
        }

        [Authorize(Roles = "Facility_Visualizar")]
        [HttpGet("/[controller]/[action]/{id}")]
        public string Lista(int id)
        {
            return "value";
        }

        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] Facility facility)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SegurancaBusiness segurancaBusiness = new SegurancaBusiness(HttpContext);

            if (string.IsNullOrEmpty(facility.Id) &&
                 !segurancaBusiness.Verifica_Acesso("Facility_Adicionar"))
            {
                return Unauthorized();
            }
            if (!string.IsNullOrEmpty(facility.Id) &&
                !segurancaBusiness.Verifica_Acesso("Facility_Editar"))
            {
                return Unauthorized();
            }
            return Ok(new FacilityBusiness(HttpContext).Salvar(facility));
        }

        [Authorize(Roles = "Facility_Editar")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> SalvaPermissao([FromBody] PermissaoFacility listaPermissao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new PermissaoBusiness(HttpContext).SalvarPermissaoFacility(listaPermissao));
        }

        [Authorize(Roles = "Facility_Editar")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> ProcessarBatch([FromBody] BatchPermissoesViewModel batchPermissoes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new PermissaoBusiness(HttpContext).ProcessarBatchFacility(batchPermissoes));
        }

        [Authorize(Roles = "Facility_Deletar")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Deleta(string Id)
        {
            return Ok(new FacilityBusiness(HttpContext).Delete(Id));
        }

        [HttpGet("/[controller]/[action]/{Id}")]
        public Prioridade PrioridadeFacility(string Id)
        {
            return new FacilityBusiness(HttpContext).PrioridadeFacility(Id);
        }
    }
}
