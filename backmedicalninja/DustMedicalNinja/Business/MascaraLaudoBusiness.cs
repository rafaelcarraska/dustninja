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
    internal class MascaraLaudoBusiness : Uteis
    {
        MascaraLaudoDao _MascaraLaudoDao = new MascaraLaudoDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;

        internal MascaraLaudoBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal Msg Insert(MascaraLaudo mascaraLaudo)
        {
            msg = new Msg();
            try
            {
                mascaraLaudo.empresaId = empresaId;
                mascaraLaudo.log = new Log().InsertLog(usuarioId);
                msg.id = _MascaraLaudoDao.Insert(mascaraLaudo).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar a máscara de laudo: {mascaraLaudo.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.MascaraLaudo, mascaraLaudo, mascaraLaudo.Id, "Insert", erro);
            }
        }

        internal void InsertList(List<MascaraLaudo> listaMascaraLaudo)
        {
            if (listaMascaraLaudo.Count > 0)
            {
                _MascaraLaudoDao.InsertList(listaMascaraLaudo);
            }
        }

        internal List<MascaraLaudo> ListAll()
        {
            return _MascaraLaudoDao.ListAll(empresaId).Result
                        .OrderBy(x => x.modalidade).ThenBy(x => x.descricao).ToList();
        }

        internal List<MascaraLaudo> ListAllAtivo()
        {
            return _MascaraLaudoDao.ListAllAtivo(empresaId).Result
                        .OrderBy(x => x.modalidade).ThenBy(x => x.descricao).ToList();
        }

        internal IEnumerable<MascaraLaudo> ListaOrderbyModalidade(string fileDcmId)
        {
            var modalidade = new FileDCMBusiness(_HttpContext).Lista(fileDcmId).modality.ToUpper().Trim();


            var listaMascaraLaudo = from r in ListAllAtivo()
                                    select new MascaraLaudo()
                        {
                            Id = r.Id,
                            descricao = r.descricao,
                            modalidade = r.modalidade.ToUpper().Trim(),
                            laudo = r.laudo
                        };

            var listaModalidade = listaMascaraLaudo
                .Where(x => x.modalidade == modalidade)
                .OrderBy(x => x.descricao);

            var listaNaoModalidae = listaMascaraLaudo
                .Where(x => x.modalidade != modalidade)
                .OrderBy(x => x.modalidade).ThenBy(x => x.descricao);

            return listaModalidade.Union(listaNaoModalidae).ToList();
        }

        internal IEnumerable<Combobox> ListCombo()
        {
            var result = _MascaraLaudoDao.ListCombo(empresaId).Result;
            return from r in result
                   orderby r.descricao
                   select new Combobox()
                   {
                       Id = r.Id,
                       descricao = r.descricao
                   };
        }

        internal MascaraLaudo List(string Id)
        {
            if (!string.IsNullOrEmpty(Id) && Id != "0") {
                List<MascaraLaudo> listaMascaraLaudo = _MascaraLaudoDao.List(Id).Result;
                if (listaMascaraLaudo.Count > 0)
                {
                    return listaMascaraLaudo.First();
                }
            }
            return new MascaraLaudo();
        }

        internal Msg Salvar(MascaraLaudo mascaraLaudo)
        {
            mascaraLaudo.empresaId = empresaId;
            msg = Validar(mascaraLaudo);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(mascaraLaudo.Id))
                {
                    return Insert(mascaraLaudo);
                }

                return Update(mascaraLaudo);
            }

            return msg;
        }

        internal Msg Update(MascaraLaudo mascaraLaudo)
        {
            msg = new Msg();
            try
            {
                mascaraLaudo.log = mascaraLaudo.log.UpdateLog(usuarioId);
                _MascaraLaudoDao.Update(mascaraLaudo);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar a máscara de laudo: {mascaraLaudo.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.MascaraLaudo, mascaraLaudo, mascaraLaudo.Id, "Update", erro);
            }
        }

        internal Msg Validar(MascaraLaudo mascaraLaudo)
        {
            List<string> erros = new List<string>();

            if (!_MascaraLaudoDao.ExisteDescricao(mascaraLaudo).Result.Equals(0))
                erros.Add("Essa descrição de máscara de laudo já existe!");

            if (string.IsNullOrEmpty(mascaraLaudo.modalidade))
            {
                erros.Add("Campo modalidade é obrigatório");
            }
            else if (mascaraLaudo.modalidade.Length > 5)
            {
                erros.Add("O campo modalidade deve conter no max. 5 caracteres.!");
            }

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Delete(string Id)
        {
            msg = new Msg();            
            try
            {
                _MascaraLaudoDao.Delete(Id);
                return msg;
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao deletar a máscara de laudo.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.MascaraLaudo, string.Empty, string.Empty, "Delete", erro);
            }            
        }

        internal bool Input(int quantidade)
        {
            try
            {
                var listaMascaraLaudo = new List<MascaraLaudo>();

                for (int x = 0; x < quantidade; x++)
                {
                    var mascaraLaudo = new MascaraLaudo
                    {
                        descricao = "Mascara " + Aleatorio(10, 2),
                        laudo = "Laudo" + Aleatorio(12, 50, true, true, true, true),
                        empresaId = empresaId,
                        status = (new Random().Next(0, 2) == 0),
                        log = new Log().InsertLog(usuarioId)
                    };

                    listaMascaraLaudo.Add(mascaraLaudo);
                }

                _MascaraLaudoDao.InsertList(listaMascaraLaudo);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Empresa, "", listaMascaraLaudo.Count(), string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return true;
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.MascaraLaudo, string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return false;
            }

        }


    }
}
