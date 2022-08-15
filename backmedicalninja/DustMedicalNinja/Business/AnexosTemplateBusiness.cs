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
    internal class AnexoTemplatesBusiness : Uteis
    {
        AnexoTemplateDao _AnexoTemplateDao = new AnexoTemplateDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;


        internal AnexoTemplatesBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());
        }

        internal List<AnexoTemplate> List()
        {
            return _AnexoTemplateDao.List(empresaId).Result;
        }

        internal AnexoTemplate ListAnexoTemplate(string anexoTemplateId)
        {
            return _AnexoTemplateDao.ListAnexoTemplate(anexoTemplateId).Result;
        }

        internal long CountAnexoTemplate(string templateImpressaoId)
        {
            return _AnexoTemplateDao.CountAnexoTemplate(templateImpressaoId).Result;
        }

        internal Msg Insert(AnexoTemplate anexoTemplate)
        {
            msg = new Msg();
            try
            {               
                anexoTemplate.log = new Log();
               
                anexoTemplate.log.InsertLog(usuarioId);
                anexoTemplate.status = true;                
                anexoTemplate.empresaId = empresaId;  

                msg.id = _AnexoTemplateDao.Insert(anexoTemplate).Result;
                new EventoBusiness(_HttpContext).Sucesso(Telas.TemplateImpressao, string.Empty, anexoTemplate, empresaId, "AnexoTemplate adicionado", $"Nome do arquivo: {anexoTemplate.descricao}, tamanho do arquivo: {anexoTemplate.tamanho}.");
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao adicionar uma anexo.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TemplateImpressao, anexoTemplate, empresaId, "Insert", erro);
            }            
        }


        internal Msg Salvar(AnexoTemplate anexoTemplate)
        {
            msg = Validar(anexoTemplate);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(anexoTemplate.Id))
                {
                    return Insert(anexoTemplate);
                }

                return Update(anexoTemplate);
            }

            return msg;
        }

        internal Msg Update(AnexoTemplate anexoTemplate)
        {
            msg = new Msg();
            try
            {
                anexoTemplate.log.UpdateLog(usuarioId);
               
                _AnexoTemplateDao.Update(anexoTemplate);

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao salvar a anexo.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TemplateImpressao, anexoTemplate, empresaId, "AnexoTemplate", erro);
            }
        }

        internal Msg Validar(AnexoTemplate anexoTemplate)
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(anexoTemplate.arquivo))
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
                var anexoTemplate = ListAnexoTemplate(Id);

                _AnexoTemplateDao.Delete(Id);
                new EventoBusiness(_HttpContext).Sucesso(Telas.TemplateImpressao, string.Empty, Id, empresaId, "AnexoTemplate removido", $"Nome do arquivo: {anexoTemplate.descricao}.");
                return msg;
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao deletar a anexo.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TemplateImpressao, string.Empty, Id, "Delete anexoTemplate", erro);
            }
        }
    }
}
