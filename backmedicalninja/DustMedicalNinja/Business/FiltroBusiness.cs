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
    internal class FiltroBusiness : Uteis
    {
        FiltroDao _FiltroDao = new FiltroDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;

        internal FiltroBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal IEnumerable<Combobox> ListCombo()
        {
            var perfilId = new UsuarioBusiness(_HttpContext).ListUsuarioLogado().perfilId;
            var result = _FiltroDao.ListCombo(empresaId, usuarioId, perfilId).Result;
           return from r in result
                   where r.listaPerfil == null 
                   || r.particular
                   || r.listaPerfil.Select(x => x.id).Contains(perfilId)
                   || r.listaPerfil == null
                   || r.listaPerfil.Count == 0
                       orderby r.particular descending, r.descricao
                   select new Combobox()
                   {
                       Id = r.Id,
                       descricao = r.descricao
                   };
        }
        
        internal IEnumerable<Combobox> ListaComboStatus()
        {
            var result = new ExameBusiness(_HttpContext).ListaStatus();
            var listaComboStatus  = from r in result
                   orderby r.statusExamesDescricao
                   select new Combobox()
                   {
                       Id = r.statusExamesFormatado,
                       descricao = r.statusExamesDescricao
                   };

            return listaComboStatus.ToList();
        }

        internal IEnumerable<Combobox> ListaComboDatas()
        {
            var listaComboDatas = new List<Combobox>();

            listaComboDatas.Add(new Combobox() {
                Id = "dataEnvio",
                descricao = "Data Envio"
            });
            listaComboDatas.Add(new Combobox()
            {
                Id = "dataExame",
                descricao = "Data do Exame"
            });

            return listaComboDatas.OrderBy(x => x.descricao);
        }

        internal IEnumerable<Combobox> ListaComboGerais()
        {
            var listaComboGerais = new List<Combobox>();

            listaComboGerais.Add(new Combobox()
            {
                Id = "modalidade",
                descricao = "Modalidade"
            });
            listaComboGerais.Add(new Combobox()
            {
                Id = "unidade",
                descricao = "Unidades"
            });

            return listaComboGerais.OrderBy(x => x.descricao);
        }


        internal IEnumerable<Combobox> ListaComboOrdem()
        {
            var listaComboOrdem = new List<Combobox>();

            listaComboOrdem.Add(new Combobox()
            {
                Id = "dataEnvio",
                descricao = "Data Envio"
            });
            listaComboOrdem.Add(new Combobox()
            {
                Id = "dataExame",
                descricao = "Data do Exame"
            });
            listaComboOrdem.Add(new Combobox()
            {
                Id = "paciente",
                descricao = "Paciente"
            });
            listaComboOrdem.Add(new Combobox()
            {
                Id = "unidade",
                descricao = "Unidade"
            });

            return listaComboOrdem.OrderBy(x => x.descricao);
        }

        internal Msg Insert(Filtro filtro)
        {
            msg = new Msg();
            try
            {
                filtro.empresaId = empresaId;
                filtro.log = new Log().InsertLog(usuarioId);
                msg.id = _FiltroDao.Insert(filtro).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao alterar configuração de filtro: {filtro.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Filtros, filtro, filtro.Id, "Insert", erro);
            }
        }

        internal void InsertList(List<Filtro> listaFiltro)
        {
            if (listaFiltro.Count > 0)
            {
                _FiltroDao.InsertList(listaFiltro);
            }
        }

        internal List<Filtro> ListAll()
        {
            return _FiltroDao.ListAll(empresaId, usuarioId).Result;
        }

        internal Filtro List(string Id)
        {
            msg = new Msg();
            try
            {
                if (!string.IsNullOrEmpty(Id) && Id != "0")
                {
                    List<Filtro> listaFiltro = _FiltroDao.List(Id).Result;
                    if (listaFiltro.Count > 0)
                    {
                        return listaFiltro.First();
                    }
                }
                return new Filtro();
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao listar filtros.";
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Filtros, string.Empty, Id, "List", erro);
                return new Filtro();
            }
            
        }

        internal Msg Salvar(Filtro filtro)
        {
            filtro.empresaId = empresaId;
            if (string.IsNullOrEmpty(filtro.Id))
            {
                filtro.usuarioId = usuarioId;
            }            
            msg = Validar(filtro);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(filtro.Id))
                {
                    return Insert(filtro);
                }

                return Update(filtro);
            }

            return msg;
        }

        internal Msg Update(Filtro filtro)
        {
            msg = new Msg();
            try
            {
                filtro.log = filtro.log.UpdateLog(usuarioId);
                _FiltroDao.Update(filtro);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o template de impressão: {filtro.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Filtros, filtro, filtro.Id, "Update", erro);
            }
        }

        internal Msg Validar(Filtro filtro)
        {
            List<string> erros = new List<string>();

            if (usuarioId != filtro.usuarioId && filtro.particular)
            {
                erros.Add("Apenas usuário que criador do filtro pode deixar o mesmo particular!");
            }

            if (!_FiltroDao.ExisteDescricao(filtro).Result.Equals(0))
            {
                erros.Add("Essa descrição do template de impressão já existe!");
            }
            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Delete(string Id)
        {
            msg = new Msg();            
            try
            {
                _FiltroDao.Delete(Id);
                return msg;
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao deletar o template de impressão.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Filtros, string.Empty, Id, "Delete", erro);
            }            
        }

        internal bool Input(int quantidade)
        {
            try
            {
                var listaFiltro = new List<Filtro>();

                for (int x = 0; x < quantidade; x++)
                {
                    var filtro = new Filtro
                    {
                        descricao = "Mascara " + Aleatorio(10, 2),
                        empresaId = empresaId,
                        status = (new Random().Next(0, 2) == 0),
                        log = new Log().InsertLog(usuarioId)
                    };

                    listaFiltro.Add(filtro);
                }

                _FiltroDao.InsertList(listaFiltro);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Empresa, "", listaFiltro.Count(), string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return true;
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Filtros, string.Empty, string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return false;
            }

        }


    }
}
