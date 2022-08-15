using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustMedicalNinja.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace DustMedicalNinja.Business
{
    internal class UsuarioCountNotaBusiness : Uteis
    {
        UsuarioCountNotaDao _UsuarioCountNotaDao = new UsuarioCountNotaDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;


        internal UsuarioCountNotaBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());
        }

        internal UsuarioCountNota List(string usuarioId, string fileDCMId)
        {
            UsuarioCountNota usuarioCountNota = _UsuarioCountNotaDao.List(usuarioId, fileDCMId).Result;           

            if (usuarioCountNota != null)
            {
                return usuarioCountNota;
            }
            return null;
        }

        internal List<UsuarioCountNota> ListUsuario()
        {
            return _UsuarioCountNotaDao.ListUsuario(usuarioId).Result;
        }

        internal long CountNotaByUsuario(List<UsuarioCountNota> listnotaUsuario, string fileDCMId)
        {
            var notaUsuario = listnotaUsuario.FirstOrDefault(x => x.fileDCMId == fileDCMId);
            if (notaUsuario != null)
            {
                return new NotasBusiness(_HttpContext).CountNotaData(fileDCMId, notaUsuario.ultimaLeitura);
            }
            return new NotasBusiness(_HttpContext).CountNota(fileDCMId);
        }

        internal Msg Insert(UsuarioCountNota usuarioCountNota)
        {
            msg = new Msg();
            try
            {
                usuarioCountNota.log = new Log();
               
                usuarioCountNota.log.InsertLog(usuarioId);
                usuarioCountNota.status = true;

                msg.id = _UsuarioCountNotaDao.Insert(usuarioCountNota).Result;
                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, usuarioCountNota, usuarioCountNota.fileDCMId, "UsuarioCountNota adicionada");
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao adicionar uma usuarioCountNota.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, usuarioCountNota, usuarioCountNota.Id, "Insert", erro);
            }            
        }
               
        internal Msg Salvar(string fileDCMId)
        {
            var usuarioCountNota = new UsuarioCountNota
            {
                fileDCMId = fileDCMId,
                ultimaLeitura = DateTime.Now,
                usuarioId = usuarioId
            };

            msg = Validar(fileDCMId);
            if (msg.erro == null)
            {
                
                if (List(usuarioId, usuarioCountNota.fileDCMId) == null)
                {
                    return Insert(usuarioCountNota);
                }

                return Update(usuarioCountNota);
            }

            return msg;
        }

        internal Msg Update(UsuarioCountNota usuarioCountNota)
        {
            msg = new Msg();
            try
            {
                usuarioCountNota.log = usuarioCountNota.log.UpdateLog(usuarioId);
                _UsuarioCountNotaDao.Update(usuarioCountNota);

                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, usuarioCountNota, usuarioCountNota.fileDCMId, "UsuarioCountNota adicionada");
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao salvar a usuarioCountNota.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, usuarioCountNota, usuarioCountNota.Id, "Update", erro);
            }
        }

        internal Msg Validar(string fileDCMId)
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(fileDCMId))
            {
                return new EventoBusiness(_HttpContext).Erro("Campo fileDCMId em branco", Telas.Worklist, string.Empty, fileDCMId, "Insert", "Erro ao adicionar uma usuarioCountNota");
            }

            return new Msg() { erro = List_Erros(erros) };
        }

       
    }
}
