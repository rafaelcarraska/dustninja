using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustMedicalNinja.Business;
using DustMedicalNinja.Context;
using DustMedicalNinja.Models;
using DustMedicalNinja.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DustMedicalNinja.Controllers
{
    [ApiController]
    public class PacienteController : Controller
    {
        public PacienteController(DCMContext context)
        {
            _context = context;
        }

        private readonly DCMContext _context;

        [Authorize(Roles = "Paciente_Visualizar")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<PacienteViewModel> Lista()
        {
            return new PacienteBusiness(HttpContext).ListAllViewModel();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaCombo()
        {
            return new PacienteBusiness(HttpContext).ListaCombo();
        }

        [Authorize(Roles = "Paciente_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public PacienteViewModel listaPaciente(string Id)
        {
            return new PacienteBusiness(HttpContext).ListaViewModel(Id);
        }

        [Authorize(Roles = "Paciente_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public IEnumerable<PacienteViewModel> ListaFacility(string Id)
        {
            return new PacienteBusiness(HttpContext).ListaFacility(Id);
        }

        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> TrocaPaciente([FromBody] TrocaPacienteViewModel trocaPaciente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new PacienteBusiness(HttpContext).TrocaPaciente(trocaPaciente, _context));
        }

        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] Paciente paciente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SegurancaBusiness segurancaBusiness = new SegurancaBusiness(HttpContext);

            //não criamos paciente novos, todos os paciente iniciar pelo DCM
            if (string.IsNullOrEmpty(paciente.Id))
            {
                return Unauthorized();
            }
            if (!string.IsNullOrEmpty(paciente.Id) &&
                !segurancaBusiness.Verifica_Acesso("Paciente_Editar"))
            {
                return Unauthorized();
            }
            return Ok(new PacienteBusiness(HttpContext).Salvar(paciente, _context));
        }

        [Authorize(Roles = "Paciente_Deletar")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Deleta(string Id)
        {
            return Ok(new PacienteBusiness(HttpContext).Delete(Id));
        }
    }
}