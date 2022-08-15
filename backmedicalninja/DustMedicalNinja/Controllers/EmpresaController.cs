using System.Collections.Generic;
using System.Threading.Tasks;
using DustMedicalNinja.Business;
using DustMedicalNinja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DustMedicalNinja.Controllers
{
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        [Authorize(Roles = "Master")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Deleta(string Id)
        {
            return Ok(new EmpresaBusiness(HttpContext).Delete(Id));
        }


        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaCombo()
        {
            return new EmpresaBusiness(HttpContext).ListaCombo();
        }

        //[AllowAnonymous]
        //[HttpGet("/[controller]/[action]")]
        //public IEnumerable<Combobox> listaComboLogin()
        //{
        //    return new EmpresaBusiness(HttpContext).listaComboLogin();
        //}

        [Authorize(Roles = "Master")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Empresa> Lista()
        {
            return new EmpresaBusiness(HttpContext).ListAll();
        }

        [HttpGet("/[controller]/[action]")]
        public Empresa EmpresaLogada()
        {
            return new EmpresaBusiness(HttpContext).EmpresaLogada();
        }

        [Authorize(Roles = "Master")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new EmpresaBusiness(HttpContext).Salvar(empresa));
        }
    }
}
