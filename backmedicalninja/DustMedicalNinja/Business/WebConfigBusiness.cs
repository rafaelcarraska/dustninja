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
    internal class WebConfigBusiness : Uteis
    {
        WebConfigDao _WebConfigDao = new WebConfigDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;


        internal WebConfigBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
        }

        internal WebConfig List()
        {
            var webConfig = _WebConfigDao.List().Result;

            if (webConfig == null)
            {
                webConfig = new WebConfig()
                {
                    sincronizando = false,
                    ultimaSincronizacao = DateTime.Now,
                    LimiteRefresh = 2,
                    timeoutSincronizacao = 5
                };
                Insert(webConfig);
                return webConfig;
            }

            return webConfig;
        }

        internal Msg Insert(WebConfig webConfig)
        {
            msg = new Msg();
            try
            {
                webConfig.log = new Log();

                webConfig.log.InsertLog(usuarioId);
                webConfig.status = true;

                msg.id = _WebConfigDao.Insert(webConfig).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao add webConfig.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, webConfig, webConfig.Id, "Insert", erro);
            }
        }

        internal Msg Update(WebConfig webConfig, bool sincronizacao)
        {
            msg = new Msg();
            try
            {

                webConfig.sincronizando = sincronizacao;                

                if (sincronizacao)
                {
                    webConfig.ultimaSincronizacao = DateTime.Now;
                }

                webConfig.log.UpdateLog(usuarioId);
                _WebConfigDao.Update(webConfig);

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao salvar a webConfig.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, sincronizacao, string.Empty, "WebConfig", erro);
            }
        }
    }
}
