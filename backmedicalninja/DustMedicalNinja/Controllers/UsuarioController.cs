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
using DustMedicalNinja.Models.ViewModel;

namespace DustMedicalNinja.Controllers
{
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [Authorize(Roles = "Usuario_Visualizar")]
        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Usuario> Lista()
        {
            return new UsuarioBusiness(HttpContext).ListAll();
        }

        [HttpGet("/[controller]/[action]")]
        public IEnumerable<Combobox> ListaCombo()
        {
            return new UsuarioBusiness(HttpContext).ListaCombo();
        }

        [Authorize(Roles = "Usuario_Visualizar")]
        [HttpGet("/[controller]/[action]/{Id}")]
        public Usuario listaUsuario(string Id)
        {
            return new UsuarioBusiness(HttpContext).List(Id);
        }
    
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> Salva([FromBody] UsuarioSenha usuarioSenha)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SegurancaBusiness segurancaBusiness = new SegurancaBusiness(HttpContext);

            Usuario usuario = usuarioSenha.usuario;
            usuario.senha = usuarioSenha.senha;

            if (string.IsNullOrEmpty(usuario.Id) &&
                 !segurancaBusiness.Verifica_Acesso("Usuario_Adicionar"))
            {
                return Unauthorized();
            }
            if (!string.IsNullOrEmpty(usuario.Id) &&
                !segurancaBusiness.Verifica_Acesso("Usuario_Editar"))
            {
                return Unauthorized();
            }            
            return Ok(new UsuarioBusiness(HttpContext).Salvar(usuario));
        }

        [Authorize(Roles = "Usuario_Editar")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> SalvaPermissao([FromBody] PermissaoUsuario listaPermissao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new PermissaoBusiness(HttpContext).SalvarPermissaoUsuario(listaPermissao));
        }

        [Authorize(Roles = "Usuario_Editar")]
        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> ProcessarBatch([FromBody] BatchPermissoesViewModel batchPermissoes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new PermissaoBusiness(HttpContext).ProcessarBatchUsuario(batchPermissoes));
        }

        [Authorize(Roles = "Usuario_Visualizar")]
        [HttpGet("/[controller]/[action]/{id}")]
        public IEnumerable<Permissao> ListaPermissao(string id)
        {
            return new PermissaoBusiness(HttpContext).List_UsuarioId(id);
        }

        [Authorize(Roles = "Usuario_Editar")]
        [HttpGet("/[controller]/[action]/{id}")]
        public TotpSetupViewModel ListaTwoFactor(string id)
        {
            return new AutenticacaoBusiness(HttpContext).ListaTwoFactor(id);
        }

        [Authorize(Roles = "Usuario_Deletar")]
        [HttpDelete("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> Deleta(string Id)
        {
            return Ok(new UsuarioBusiness(HttpContext).Delete(Id));
        }

        [HttpGet("/[controller]/[action]/{Id}")]
        public async Task<IActionResult> NovoTwoFactor(string Id)
        {
            return Ok(new UsuarioBusiness(HttpContext).NovoTwoFactor(Id));
        }

        [HttpPut("/[controller]/[action]")]
        public async Task<IActionResult> ValidaTwoFactor([FromBody] ValidaTwoFactorViewModel validaTwoFactor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new AutenticacaoBusiness(HttpContext).ValidaTwoFactor(validaTwoFactor));
        }

    }
}
