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
using DustMedicalNinja.Models.ViewModel;
using DustMedicalNinja.Models.ViewModel;
using DustMedicalNinja.Models.AiModel;

namespace DustMedicalNinja.Business
{
    internal class DesmembrarBusiness : Uteis
    {
        DesmembrarAiDao _DesmembrarAiDao = new DesmembrarAiDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;


        internal DesmembrarBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
        }

        internal Msg InsertAi(DesmembrarViewModel desmembrarViewModel, string descricaoAntiga)
        {
            msg = new Msg();
            try
            {
                DesmembrarAi desmembrarAi = new DesmembrarAi()
                {
                    fileDCMId = desmembrarViewModel.fileDCMId,
                    novaDescricao = desmembrarViewModel.novaDescricao,
                    descricaoAntiga = descricaoAntiga,
                    novosExames = desmembrarViewModel.novosExames,

                    empresaId = empresaId,
                    log = new Log().InsertLog(usuarioId),
                    status = true
                };

                msg.id = _DesmembrarAiDao.Insert(desmembrarAi).Result;
                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, desmembrarAi, desmembrarAi.fileDCMId, "Analise de Ai.");

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao salvar Analise de Ai desmembramento.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, desmembrarViewModel.fileDCMId, desmembrarViewModel.fileDCMId, "DesmembrarAi", erro);
            }
        }
    }
}
