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
    public class PerfilController : ControllerBase
    {
        [Authorize(Roles = "Master")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Perfil> Lista()
        {
            return new PerfilBusiness(HttpContext).ListAll();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaCombo()
        {
            return new PerfilBusiness(HttpContext).ListCombo();
        }

        [Authorize(Roles = "Master")]
        [HttpGet("/[controller]/[action]/{id}")]
        public string Lista(int id)
        {           
            return "value";
        }

        [Authorize(Roles = "Master")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] Perfil perfil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new PerfilBusiness(HttpContext).Salvar(perfil));
        }

        [Authorize(Roles = "Master")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Deleta(string Id)
        {
            return Ok(new PerfilBusiness(HttpContext).Delete(Id));
        }
    }
}
