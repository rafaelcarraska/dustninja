using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustMedicalNinja.Extensions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DustMedicalNinja.Business
{
    internal class ConfiguracaoBusiness : Uteis
    {
        ConfiguracaoDao _ConfiguracaoDao = new ConfiguracaoDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string usuarioId;

        internal ConfiguracaoBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal Msg Insert(Configuracao configuracao)
        {
            msg = new Msg();
            try
            {
                configuracao.usuarioId = usuarioId;
                configuracao.status = true;
                configuracao.log = new Log().InsertLog(usuarioId);
                msg.id = _ConfiguracaoDao.Insert(configuracao).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar a Configuracao.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, configuracao, configuracao.usuarioId, "Insert", erro);
            }
        }

        internal Configuracao List()
        {
            try
            {
                if (!string.IsNullOrEmpty(usuarioId) && usuarioId != "0")
                {
                    var configuracao = _ConfiguracaoDao.List(usuarioId).Result;
                    if (configuracao != null)
                    {
                        return configuracao;
                    }
                    return new Configuracao()
                    {
                        pageSize = 5
                    };
                }
                return new Configuracao();
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao listar a Configuracao.";
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, string.Empty, string.Empty);
                return new Configuracao();
            }
        }

        internal Msg Salvar(Configuracao configuracao)
        {
            configuracao.usuarioId = usuarioId;
            msg = Validar(configuracao);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(configuracao.Id))
                {
                    return Insert(configuracao);
                }

                return Update(configuracao);
            }

            return msg;
        }

        internal Msg Update(Configuracao configuracao)
        {
            msg = new Msg();
            try
            {
                configuracao.log = configuracao.log.UpdateLog(usuarioId);
                _ConfiguracaoDao.Update(configuracao);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar aConfiguração.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, configuracao, configuracao.usuarioId, "Update", erro);
            }
        }

        internal Msg Validar(Configuracao configuracao)
        {
            List<string> erros = new List<string>();

            if (configuracao.pageSize <= 0)
                erros.Add("A quantidade de itens por página deve ser maior que zero!");

            if (string.IsNullOrEmpty(configuracao.usuarioId))
                erros.Add("Erro ao validar o usuário!");

            return new Msg() { erro = List_Erros(erros) };
        }


    }
}
