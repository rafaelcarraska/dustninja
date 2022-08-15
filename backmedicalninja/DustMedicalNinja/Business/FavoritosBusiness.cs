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
    internal class FavoritosBusiness : Uteis
    {
        FavoritosDao _FavoritosDao = new FavoritosDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;


        internal FavoritosBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());
        }

        internal Favoritos ListFileCDMId(string filedcmId)
        {
            Favoritos favoritos = _FavoritosDao.ListFileCDMId(usuarioId, filedcmId).Result;           

            if (favoritos != null)
            {
                return favoritos;
            }
            return null;
        }

        internal List<Favoritos> List()
        {
            List<Favoritos> favoritos = _FavoritosDao.List(usuarioId).ToList();

            if (favoritos != null)
            {
                return favoritos;
            }
            return new List<Favoritos>();
        }

        internal Msg Insert(string fileDCMId)
        {
            msg = new Msg();
            try
            {
                Favoritos favoritos = new Favoritos()
                {
                    usuarioId = usuarioId,
                    empresaId = empresaId,
                    filedcmId = fileDCMId,
                    log = new Log().InsertLog(usuarioId),
                    status = true
                };

                if (ListFileCDMId(fileDCMId) == null)
                {
                    msg.id = _FavoritosDao.Insert(favoritos).Result;
                    new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, favoritos, fileDCMId, "Exame adicionado aos favoritos.");
                }
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao favoritar o exame.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, fileDCMId, fileDCMId, "Favoritar", erro);
            }
        }


        internal Msg Salvar(string fileDCMId)
        {
            msg = Validar(fileDCMId);
            if (msg.erro == null)
            {
                if (ListFileCDMId(fileDCMId) != null )
                {
                    return Delete(fileDCMId);
                }

                return Insert(fileDCMId);
            }

            return msg;
        }

        internal Msg Validar(string fileDCMId)
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(fileDCMId))
            {
                msg.erro.Add($"Erro ao favoritar o exame.");
            }

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Delete(string fileDCMId)
        {
            msg = new Msg();           
            try
            {
                _FavoritosDao.Delete(usuarioId, fileDCMId);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, string.Empty, fileDCMId, "Exame removido dos favoritos.");
                return msg;
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao removar favorito.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, string.Empty, fileDCMId, "favorito", erro);
            }
        }
    }
}
