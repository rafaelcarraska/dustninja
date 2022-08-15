using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DustMedicalNinja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {
                $" Dust Medical Ninja, ambiente de { Startup.ambiente } ",
                $" Data/Hora do Servidor: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt")} ",
                $" Versão: 1.0.5.9 "            };
        }
    }
}
