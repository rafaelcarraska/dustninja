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
    public class SistemaController : ControllerBase
    {
        //[Authorize(Roles = "Master")]
        [AllowAnonymous]
        [HttpGet("/[controller]/[action]/{tela}")]
        public string Input(string tela)
        {
            var arr = tela.Split('_');
            int quantidade = arr.Length > 1 ? Convert.ToInt16(arr[1]) : 30;
            List<string> msg = new List<string>();

            switch (arr[0])
            {
                case "empresa":
                    msg.Add("'Empresa': " + new EmpresaBusiness(HttpContext).Input(quantidade));
                    break;
                case "perfil":
                    msg.Add("'Perfil': " + new PerfilBusiness(HttpContext).Input(quantidade));
                    break;
                case "usuario":
                    msg.Add("'Usuario': " + new UsuarioBusiness(HttpContext).Input(quantidade));
                    break;
                case "facility":
                    msg.Add("'Facility': " + new FacilityBusiness(HttpContext).Input(quantidade));
                    break;
                case "mascaraLaudo":
                    msg.Add("'Mascara de Laudo': " + new MascaraLaudoBusiness(HttpContext).Input(quantidade));
                    break;
                case "all":
                    msg.Add("'Empresa': " + new EmpresaBusiness(HttpContext).Input(quantidade));
                    msg.Add("'Perfil': " + new PerfilBusiness(HttpContext).Input(quantidade));
                    msg.Add("'Usuario': " + new UsuarioBusiness(HttpContext).Input(quantidade));
                    msg.Add("'Facility': " + new FacilityBusiness(HttpContext).Input(quantidade));
                    msg.Add("'Mascara de Laudo': " + new MascaraLaudoBusiness(HttpContext).Input(quantidade));
                    break;
                default:
                    msg.Add("Tela invalida.");
                    break;
            }

            return "{ " + string.Join(", ", msg) + " }";

        }

        //[AllowAnonymous]
        //[HttpGet("/[controller]/[action]")]
        //public string Restore()
        //{
        //    new BackupBusiness().Restore();

        //    return "ok";

        //}

        //[AllowAnonymous]
        //[HttpGet("/[controller]/[action]")]
        //public string Backup()
        //{
        //    new BackupBusiness().Backup();

        //    return "ok";

        //}
    }
}
