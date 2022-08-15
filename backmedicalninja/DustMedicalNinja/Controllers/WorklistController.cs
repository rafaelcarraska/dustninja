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
    public class WorklistController : ControllerBase
    {
        public WorklistController(DCMContext context)
        {
            _context = context;
        }

        private readonly DCMContext _context;

        [Authorize(Roles = "Worklist_Visualizar")]
        //[AllowAnonymous]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<FileDCMViewModel> Lista()
        {
            return new FileDCMBusiness(HttpContext).List_DCM4CHEE(_context);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public IEnumerable<FileDCMViewModel> Lista(string Id)
        {
            return new FileDCMBusiness(HttpContext).List_DCM4CHEEFiltroPage(_context, Id);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public IEnumerable<FileDCMViewModel> ListaFiltro(string Id)
        {
            return new FileDCMBusiness(HttpContext).List_DCM4CHEE_Filtro(Id);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public FileDCMViewModel Listaexame(string Id)
        {
            return new FileDCMBusiness(HttpContext).ListaExame(Id);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public NotaViewModel listaNotas(string Id)
        {
            return new NotasBusiness(HttpContext).ListViewModelUsuario(Id);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> DeletarNota([FromBody] Nota nota)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new NotasBusiness(HttpContext).DeleteNota(nota));
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public IEnumerable<EventoExameViewModel> listaEventos(string Id)
        {
            return new EventoBusiness(HttpContext).ListEventos_FileDCM_LogUsuario(Id);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public IEnumerable<Anexo> listaAnexos(string Id)
        {
            return new AnexosBusiness(HttpContext).List(Id);
        }

        [Authorize(Roles = "Worklist_Editar")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> DeletarAnexo(string Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new AnexosBusiness(HttpContext).Delete(Id));
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> SalvaNota([FromBody] FileDCMNota fileDCMNota)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new NotasBusiness(HttpContext).Salvar(fileDCMNota));
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpPost("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Favoritar(string Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new FavoritosBusiness(HttpContext).Salvar(Id));
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public LaudoPageViewModel GetLaudoPage(string Id)
        {
            var query = Id.Split("_");
            var fileDCMId = query[0];
            var templateImpressaoId = (query.Count() > 1 ? query[1] : string.Empty);

            return new FileDCMBusiness(HttpContext).GetLaudoPage(fileDCMId, templateImpressaoId);
        }
    }
}