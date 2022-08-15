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
    internal class AnexosBusiness : Uteis
    {
        AnexoDao _AnexoDao = new AnexoDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;


        internal AnexosBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());
        }

        internal List<Anexo> List(string fileDCMId)
        {
            return _AnexoDao.List(fileDCMId).Result;
        }

        internal Anexo ListAnexo(string anexoId)
        {
            return _AnexoDao.ListAnexo(anexoId).Result;
        }

        internal long CountAnexo(string fileDCMId)
        {
            return _AnexoDao.CountAnexo(fileDCMId).Result;
        }

        internal long CountAnexoByList(List<Anexo> listAnexo, string fileDCMId)
        {
            return listAnexo.Count(x => x.fileDCMId == fileDCMId);
        }

        internal Msg Insert(Anexo anexo)
        {
            msg = new Msg();
            try
            {               
                anexo.log = new Log();
               
                anexo.log.InsertLog(usuarioId);
                anexo.status = true;                
                anexo.empresaId = empresaId;  

                msg.id = _AnexoDao.Insert(anexo).Result;
                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, anexo, anexo.fileDCMId, "Anexo adicionado", $"Nome do arquivo: {anexo.descricao}, tamanho do arquivo: {anexo.tamanho}.");
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao adicionar uma anexo.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, anexo, anexo.fileDCMId, "Insert", erro);
            }            
        }


        internal Msg Salvar(Anexo anexo)
        {
            msg = Validar(anexo);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(anexo.Id))
                {
                    return Insert(anexo);
                }

                return Update(anexo);
            }

            return msg;
        }

        internal Msg Update(Anexo anexo)
        {
            msg = new Msg();
            try
            {
                anexo.log.UpdateLog(usuarioId);
               
                _AnexoDao.Update(anexo);

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao salvar a anexo.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, anexo, anexo.fileDCMId, "Anexo", erro);
            }
        }

        internal List<Anexo> ListaAnexoByFileDCMId(List<string> listaFileDCMId)
        {
            if (listaFileDCMId != null)
            {
                return _AnexoDao.ListaAnexoByFileDCMId(listaFileDCMId).Result;
            }

            return new List<Anexo>();
        }

        internal Msg Validar(Anexo anexo)
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(anexo.arquivo))
            {
                msg.erro.Add($"O nome do arquivo é obrigatório.");
            }

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Delete(string Id)
        {
            msg = new Msg();           
            try
            {
                var anexo = ListAnexo(Id);

                _AnexoDao.Delete(Id);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, Id, anexo.fileDCMId, "Anexo removido", $"Nome do arquivo: {anexo.descricao}.");
                return msg;
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao deletar a anexo.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, string.Empty, Id, "Delete anexo", erro);
            }
        }
    }
}
