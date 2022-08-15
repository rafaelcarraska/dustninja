
using DustMedicalNinja.Business;
using DustMedicalNinja.Context;
using DustMedicalNinja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DustMedicalNinja.Controllers
{
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        public RelatorioController(DCMContext context)
        {
            _context = context;
        }

        private readonly DCMContext _context;

        [Authorize(Roles = "Relatorio_Visualizar")]
        [HttpPost("/[controller]/[action]")]
        public IActionResult getRelatorioGerencial([FromBody] RelatorioCSV relatorioCSV)
        {
            var fileBinary = new RelatorioBusiness(HttpContext).getRelatorioGerencial(relatorioCSV);

            new FileExtensionContentTypeProvider().TryGetContentType("relatorioGerencial.csv", out var contentType);

            return File(fileBinary, contentType, $"relatorioGerencial.csv");
        }
    }
}