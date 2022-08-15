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
    internal class TipoExameBusiness : Uteis
    {
        TipoExameDao _TipoExameDao = new TipoExameDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;

        internal TipoExameBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal Msg Insert(TipoExame tipoExame)
        {
            msg = new Msg();
            try
            {
                tipoExame.empresaId = empresaId;
                tipoExame.log = new Log().InsertLog(usuarioId);
                msg.id = _TipoExameDao.Insert(tipoExame).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar a máscara de laudo: {tipoExame.nome}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TipoExame, tipoExame, tipoExame.Id, "Insert", erro);
            }
        }

        internal void InsertList(List<TipoExame> listaTipoExame)
        {
            if (listaTipoExame.Count > 0)
            {
                _TipoExameDao.InsertList(listaTipoExame);
            }
        }

        internal List<TipoExame> ListAll()
        {
            return _TipoExameDao.ListAll(empresaId).Result;
        }

        internal TipoExame List(string Id)
        {
            if (!string.IsNullOrEmpty(Id) && Id != "0") {
                List<TipoExame> listaTipoExame = _TipoExameDao.List(Id).Result;
                if (listaTipoExame.Count > 0)
                {
                    return listaTipoExame.First();
                }
            }
            return new TipoExame();
        }

        internal IEnumerable<Combobox> ListCombo()
        {
            var result = _TipoExameDao.ListCombo(empresaId).Result;
            return from r in result
                   orderby r.nome
                   select new Combobox()
                   {
                       Id = r.Id,
                       descricao = r.nome
                   };
        }

        internal IEnumerable<Combobox> ListaTipoEstudo(string modalidade)
        {
            modalidade = modalidade.ToUpper().Trim();
           var listaTipoEstudo =  from r in _TipoExameDao.listaTipoEstudo(empresaId).Result
            select new Combobox()
                   {
                       Id = r.Id,
                       descricao = r.nome,
                       filtro1 = r.modalidade.ToUpper().Trim()
            };

            var listaModalidade = listaTipoEstudo
                .Where(x => x.filtro1 == modalidade)
                .OrderBy(x => x.descricao);

            var listaNaoModalidae = listaTipoEstudo
                .Where(x => x.filtro1 != modalidade)
                .OrderBy(x => x.filtro1).ThenBy(x => x.descricao);

            return listaModalidade.Union(listaNaoModalidae).ToList();
        }

        internal Msg Salvar(TipoExame tipoExame)
        {
            tipoExame.empresaId = empresaId;
            msg = Validar(tipoExame);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(tipoExame.Id))
                {
                    return Insert(tipoExame);
                }

                return Update(tipoExame);
            }

            return msg;
        }

        internal Msg Update(TipoExame tipoExame)
        {
            msg = new Msg();
            try
            {
                tipoExame.log = tipoExame.log.UpdateLog(usuarioId);
                _TipoExameDao.Update(tipoExame);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar a máscara de laudo: {tipoExame.nome}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TipoExame, tipoExame, tipoExame.Id, "Update", erro);
            }
        }

        internal Msg Validar(TipoExame tipoExame)
        {
            List<string> erros = new List<string>();

            if (!_TipoExameDao.ExisteDescricao(tipoExame).Result.Equals(0))
            {
                erros.Add("Essa descrição de máscara de laudo já existe!");
            }

            if (string.IsNullOrEmpty(tipoExame.mascaraLaudoId))
            {
                erros.Add("Obrigatório selecionar uma máscara de laudo!");
            }

            if (string.IsNullOrEmpty(tipoExame.modalidade))
            {
                erros.Add("Campo modalidade é obrigatório");
            }else if (tipoExame.modalidade.Length > 5)
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
                _TipoExameDao.Delete(Id);
                return msg;
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao deletar a máscara de laudo.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TipoExame, string.Empty, Id, "Delete", erro);
            }            
        }

        //internal bool Input(int quantidade)
        //{
        //    try
        //    {
        //        var listaTipoExame = new List<TipoExame>();

        //        for (int x = 0; x < quantidade; x++)
        //        {
        //            var tipoExame = new TipoExame
        //            {
        //                nome = "Mascara " + Aleatorio(10, 2),
        //                mascaraLaudoId = 0,
        //                empresaId = empresaId,
        //                status = (new Random().Next(0, 2) == 0),
        //                log = new Log().InsertLog(usuarioId)
        //            };

        //            listaTipoExame.Add(tipoExame);
        //        }

        //        _TipoExameDao.InsertList(listaTipoExame);
        //        new EventoBusiness(_HttpContext).Sucesso(Telas.Empresa, "", listaTipoExame.Count(), "Input", "quantidade solicitada: " + quantidade);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TipoExame, "Input", "quantidade solicitada: " + quantidade);
        //        return false;
        //    }

        //}


    }
}
