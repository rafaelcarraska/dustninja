using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DustMedicalNinja.Models.ViewModel;

namespace DustMedicalNinja.Business
{
    internal class EventoBusiness : Uteis
    {
        EventoDao _EventoDao = new EventoDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;

        internal EventoBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal List<EventoExameViewModel> ListEventos_FileDCM_LogUsuario(string fileDCMId, bool filtraLogUsuario = true)
        {
            var eventos = _EventoDao.ListEventos_FileDCM_LogUsuario(fileDCMId, Telas.Worklist.ToString("g")).Result;
            var usuario = new UsuarioBusiness(_HttpContext).UsuarioView();
            
            var eventoVM = (from e in eventos
                            join u in usuario on e.usuarioId equals u.Id
                            orderby e.data descending
                            select new EventoExameViewModel
                             {
                                 usuario = u.nome,
                                 perfil = u.perfil,
                                 data = e.data,
                                 acao = e.acao,
                                 Obs = e.Obs
                             }).ToList();

            return eventoVM;
        }

        internal Msg Erro(string exception, Telas tela, object valorNovo, string itemId, string acao = "", string obs = "")
        {
            try
            {
                Msg msg = new Msg();
                msg.erro = new List<string>();
                msg.erro.Add(obs);

                var evento = new Evento()
                {
                    empresaId = empresaId,
                    usuarioId = usuarioId,
                    data = DateTime.Now,
                    tela = tela.ToString("g"),
                    acao = acao, 
                    valorNovo = valorNovo,
                    erro = true,
                    Obs = obs,
                    exception = exception,
                    itemId = itemId
                };

                _EventoDao.Insert(evento);

                return msg;
            }
            catch (Exception)
            {
                Msg msg = new Msg();
                msg.erro = new List<string>();
                msg.erro.Add("Erro ao registar o log. Entre em contato com o administrador.");
                return msg;
            }
        }

        internal void Sucesso(Telas tela, object valorAnterio, object valorNovo, string itemId, string acao = "", string obs = "", bool logUsuario = true)
        {
            try
            {
                var evento = new Evento()
                {
                    empresaId = empresaId,
                    usuarioId = usuarioId,
                    data = DateTime.Now,
                    tela = tela.ToString("g"),
                    acao = acao,
                    valorAnterio = JsonConvert.SerializeObject(valorAnterio),
                    valorNovo = JsonConvert.SerializeObject(valorNovo),
                    erro = false,
                    Obs = obs,
                    itemId = itemId,
                    logUsuario = logUsuario
                };

                _EventoDao.Insert(evento);
            }
            catch (Exception)
            {

            }
        }

        internal void Login(object valorNovo)
        {
            try
            {
                var evento = new Evento()
                {
                    empresaId = empresaId,
                    usuarioId = usuarioId,
                    data = DateTime.Now,
                    tela = "Login",
                    acao = "Logar",
                    valorNovo = JsonConvert.SerializeObject(valorNovo),
                    erro = false
                };

                _EventoDao.Insert(evento);
            }
            catch (Exception)
            {

            }
        }
    }
}
