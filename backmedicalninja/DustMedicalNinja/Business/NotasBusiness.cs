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

namespace DustMedicalNinja.Business
{
    internal class NotasBusiness : Uteis
    {
        NotaDao _NotaDao = new NotaDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;


        internal NotasBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());
        }

        internal Nota List(string fileDCMId)
        {
            Nota nota = _NotaDao.List(fileDCMId).Result;

            if (nota != null)
            {
                return nota;
            }
            return null;
        }

        internal Nota ListNota(string notaId)
        {
            Nota nota = _NotaDao.List(notaId).Result;

            if (nota != null)
            {
                return nota;
            }
            return null;
        }

        internal long CountNota(string fileDCMId)
        {
            return _NotaDao.CountNota(fileDCMId, usuarioId);
        }

        internal bool Existe(string fileDCMId)
        {
            return _NotaDao.Existe(fileDCMId).Result;
        }

        internal long CountNotaData(string fileDCMId, DateTime data)
        {
            return _NotaDao.CountNotaData(fileDCMId, data, usuarioId);
        }

        internal NotaViewModel ListViewModelUsuario(string fileDCMId)
        {
            new UsuarioCountNotaBusiness(_HttpContext).Salvar(fileDCMId);
            return ListViewModel(fileDCMId);
        }

        internal NotaViewModel ListViewModel(string fileDCMId)
        {
            Nota nota = _NotaDao.List(fileDCMId).Result;

            if (nota != null)
            {
                var usuario = new UsuarioBusiness(_HttpContext).UsuarioView();

                NotaViewModel notaVM = new NotaViewModel();

                notaVM.Id = nota.Id;
                notaVM.log = nota.log;
                notaVM.status = nota.status;
                notaVM.fileDCMId = nota.fileDCMId;

                notaVM.listaNota = (from n in nota.listaNota
                                    join u in usuario on n.usuarioId equals u.Id
                                    select new NotaHistoricoViewModel()
                                    {
                                        nota = n.nota,
                                        usuarioId = n.usuarioId,
                                        usuario = u.nome,
                                        perfil = u.perfil,
                                        data = n.data
                                    }).ToList();


                if (notaVM != null)
                {
                    return notaVM;
                }
            }
            return null;
        }

        internal Msg Insert(FileDCMNota fileDCMNota)
        {
            msg = new Msg();
            try
            {
                Nota nota = new Nota();
                nota.log = new Log();

                nota.log.InsertLog(usuarioId);
                nota.status = true;
                nota.fileDCMId = fileDCMNota.fileDCMId;
                nota.empresaId = empresaId;
                nota.listaNota = new List<NotaHistorico>();

                NotaHistorico notaHistorico = new NotaHistorico();
                notaHistorico.usuarioId = usuarioId;
                notaHistorico.data = DateTime.Now;
                notaHistorico.nota = fileDCMNota.nota;

                nota.listaNota.Add(notaHistorico);

                new FavoritosBusiness(_HttpContext).Insert(fileDCMNota.fileDCMId);

                msg.id = _NotaDao.Insert(nota).Result;
                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, notaHistorico, fileDCMNota.fileDCMId, "Nota adicionada");
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao adicionar uma nota.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, fileDCMNota, fileDCMNota.Id, "Insert", erro);
            }
        }


        internal Msg Salvar(FileDCMNota fileDCMNota)
        {
            msg = Validar(fileDCMNota);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(fileDCMNota.Id))
                {
                    return Insert(fileDCMNota);
                }

                return Update(fileDCMNota);
            }

            return msg;
        }

        internal Msg Update(FileDCMNota fileDCMNota)
        {
            msg = new Msg();
            try
            {
                Nota nota = List(fileDCMNota.fileDCMId);
                if (nota == null)
                {
                    Insert(fileDCMNota);
                }
                nota.log.UpdateLog(usuarioId);

                NotaHistorico notaHistorico = new NotaHistorico();
                notaHistorico.usuarioId = usuarioId;
                notaHistorico.data = DateTime.Now;
                notaHistorico.nota = fileDCMNota.nota;

                nota.listaNota.Add(notaHistorico);
                _NotaDao.Update(nota);

                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, notaHistorico, fileDCMNota.fileDCMId, "Nota adicionada");
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao salvar a nota.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, fileDCMNota, fileDCMNota.Id, "Update", erro);
            }
        }

        internal Msg Validar(FileDCMNota fileDCMNota)
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(fileDCMNota.nota))
            {
                msg.erro.Add($"O campo nota é obrigatório.");
            }

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg DeleteNota(Nota nota)
        {
            msg = new Msg();
            try
            {
                var notaAntiga = List(nota.fileDCMId);
                var difereca = new List<NotaHistorico>();

                //TODO add ID no notaHistorico e passa des do front ateh essa validacao pelo id
                foreach (var notaHist in notaAntiga.listaNota)
                {
                    if (!nota.listaNota.Any(x => x.data == notaHist.data))
                    {
                        difereca.Add(notaHist);
                    }
                }

                if (difereca.Count() == 1)
                {
                    var permissoes = new PermissaoBusiness(_HttpContext).List_Permissao_Workilist().ToList();
                    var fileDCM = new FileDCMBusiness(_HttpContext).Lista(nota.fileDCMId);
                    var facility = new FacilityBusiness(_HttpContext).ListaEaTitle(fileDCM.aeTitle);


                    if (difereca.FirstOrDefault().usuarioId == usuarioId || new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, facility.Id, "Remover notas"))
                    {
                        notaAntiga.log.UpdateLog(usuarioId);
                        notaAntiga.listaNota = nota.listaNota;

                        _NotaDao.Update(nota);
                        new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, nota.Id, nota.fileDCMId, "Nota removida");
                        return msg;
                    }

                }

                msg.erro = new List<string>();
                string erro = $"Erro ao deletar a nota.";
                return new EventoBusiness(_HttpContext).Erro("erro ao validar o processo de remover nota", Telas.Worklist, string.Empty, nota.Id, "Delete nota", erro);
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao deletar a nota.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, string.Empty, nota.Id, "Delete nota", erro);
            }
        }
    }
}
