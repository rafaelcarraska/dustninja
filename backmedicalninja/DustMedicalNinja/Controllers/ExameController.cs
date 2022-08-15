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
    public class ExameController : ControllerBase
    {
        public ExameController(DCMContext context)
        {
            _context = context;
        }

        private readonly DCMContext _context;

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public StatusExameViewModel VerificaStatusExame(string Id)
        {
            return new ExameBusiness(HttpContext).verificaStatusExame(Id);
        }

        [AllowAnonymous]
        [HttpGet("/[controller]/[action]/{Id}")]
        public string ConfirmacaoStatusExame(string Id)
        {
            return new ExameBusiness(HttpContext).ConfirmacaoStatusExame(Id);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Confirmacao([FromBody] Confirmacao confirmacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new ExameBusiness(HttpContext).Confirmacao(confirmacao));
        }

        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Desmembrar([FromBody] DesmembrarViewModel desmembrar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new ExameBusiness(HttpContext).Desmembrar(desmembrar));
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> LogVisualizacao([FromBody] LogVisualizacao logVisualizacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new ExameBusiness(HttpContext).LogVisualizacao(logVisualizacao));
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public ExameInfoViewModel ListaExameInfo(string Id)
        {
            return new ExameBusiness(HttpContext).ListaExameInfo(Id, StatusExames.laudando);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public ExameInfoViewModel ListaExameInfoLaudado(string Id)
        {
            return new ExameBusiness(HttpContext).ListaExameInfo(Id, StatusExames.laudado);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public bool VerificaRevisao(string Id)
        {
            return new ExameBusiness(HttpContext).VerificaRevisao(Id);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public Confirmacao CarregaUltimoLaudo(string Id)
        {
            return new ExameBusiness(HttpContext).CarregaUltimoLaudo(Id);
        }

        [Authorize(Roles = "Worklist_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public NotaHistoricoViewModel HistoricoClinicoLoad(string Id)
        {
            return new ExameBusiness(HttpContext).HistoricoClinicoLoad(Id);
        }

        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] FileDCM fileDCM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SegurancaBusiness segurancaBusiness = new SegurancaBusiness(HttpContext);

            //não criamos paciente novos, todos os paciente iniciar pelo DCM
            if (string.IsNullOrEmpty(fileDCM.Id))
            {
                return Unauthorized();
            }
            if (!string.IsNullOrEmpty(fileDCM.Id) &&
                !segurancaBusiness.Verifica_Acesso("Worklist_Editar"))
            {
                return Unauthorized();
            }
            return Ok(new ExameBusiness(HttpContext).Salvar(fileDCM, _context));
        }
    }
}