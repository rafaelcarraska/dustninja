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
    internal class EmpresaBusiness : Uteis
    {
        EmpresaDao _EmpresaDao = new EmpresaDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;

        internal EmpresaBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal Msg Insert(Empresa empresa)
        {
            msg = new Msg();
            try
            {
                empresa.log = new Log().InsertLog(usuarioId);
                empresa.listaTela = new AcessoBusiness(_HttpContext).InputTela();
                msg.id = _EmpresaDao.Insert(empresa).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar a empresa: {empresa.nomeFantasia}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Empresa, empresa, empresa.Id, "Insert", erro);
            }
        }

        internal void InsertList(List<Empresa> listEmpresa)
        {
            if (listEmpresa.Count > 0)
            {
               _EmpresaDao.InsertList(listEmpresa);
            }
        }

        internal IEnumerable<Empresa> ListAll()
        {
            var result = _EmpresaDao.ListAll().Result;
            return from r in result
                   orderby r.status descending, r.nomeFantasia
                   select r;
        }

        internal IEnumerable<Combobox> ListaCombo()
        {
            var result = _EmpresaDao.ListaCombo().Result;
            return from r in result
                   orderby r.status descending, r.nomeFantasia
                   select new Combobox {
                       Id = r.Id,
                       descricao = r.nomeFantasia
                   };
        }

        internal IEnumerable<Combobox> listaComboLogin(List<string> listaEmpresa)
        {
            var result = _EmpresaDao.listaComboLogin(listaEmpresa).Result;
            return from r in result
                   orderby r.status descending, r.nomeFantasia
                   select new Combobox
                   {
                       Id = r.Id,
                       descricao = r.nomeFantasia
                   };
        }

        internal Empresa Lista(string Id)
        {
            List<Empresa> listaEmpresa = _EmpresaDao.Lista(Id).Result;
            if (listaEmpresa.Count > 0)
            {
                return listaEmpresa.First();
            }
            return new Empresa();
        }

        internal Empresa ListaEmpresaLogada()
        {
            return Lista(empresaId);
        }

        internal Empresa EmpresaLogada()
        {
            return _EmpresaDao.Lista(empresaId).Result.FirstOrDefault();
        }

        internal Msg Salvar(Empresa empresa)
        {
            msg = Validar(empresa);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(empresa.Id))
                {
                    return Insert(empresa);
                }

                return Update(empresa);
            }

            return msg;
        }

        internal Msg Update(Empresa empresa)
        {
            msg = new Msg();
            try
            {
                empresa.log = empresa.log.UpdateLog(usuarioId);
                empresa.listaTela = new AcessoBusiness(_HttpContext).InputTela();
                _EmpresaDao.Update(empresa);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar a empresa: {empresa.nomeFantasia}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Empresa, empresa, empresa.Id, "Update", erro);
            }
        }

        internal Msg Validar(Empresa empresa)
        {
            List<string> erros = new List<string>();

            if (!_EmpresaDao.ExisteRazaoSocial(empresa).Result.Equals(0))
                erros.Add("Essa razão social já existe!");

            if (!_EmpresaDao.ExisteNomeFantasia(empresa).Result.Equals(0))
                erros.Add("Esse nome fantasia já existe!");

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Delete(string Id)
        {
            msg = new Msg();
            var listaErro = new List<string>();
            try
            {
                if (!EmUso(Id))
                {
                    _EmpresaDao.Delete(Id);
                    return msg;
                }
                var msgInativar = Inativar(Id);
                if (string.IsNullOrEmpty(msgInativar))
                {
                    listaErro.Add($"Não é possivel deletar essa empresa, pois ele está em uso.");
                    listaErro.Add($"Status da empresa alterado para Inativo.");
                }
                listaErro.Add(msgInativar);

                msg.erro = listaErro;
                msg.status = false;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao deletar a empresa.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Empresa, "", Id, "Delete", erro);
            }            
        }

        internal string Inativar(string Id)
        {
            try
            {
                Empresa empresa = Lista(Id);

                if (empresa.status)
                {
                    empresa.status = false;
                    Update(empresa);
                }

                return null;
            }
            catch (Exception)
            {
                return $"Erro ao inativar a empresa.";
            }
        }

        internal bool EmUso(string Id)
        {
            if (new UsuarioBusiness(_HttpContext).EmpresaEmUso(Id) > 0)
            {
                return true;
            }
            if (new FacilityBusiness(_HttpContext).ListaFacilityCount(Id) > 0)
            {
                return true;
            }
            return false;
        }

        internal bool Input(int quantidade)
        {
            try
            {
                var listEmpresa = new List<Empresa>();

                for (int x = 0; x < quantidade; x++)
                {
                    var empresa = new Empresa
                    {
                        razaoSocial = "razao social " + Aleatorio(10, 2),
                        nomeFantasia = "nome fantasia "+ Aleatorio(10, 3),
                        status = (new Random().Next(0,2) == 0),
                        endereco = new EnderecoBusiness().Input(),
                        cnpj = "00.000.000/00001-00",
                        responsavel = "responsavel " + Aleatorio(8, 2, true, true, true, false),
                        listaTela = new AcessoBusiness(_HttpContext).InputTela(),
                        listaContato = new ContatoBusiness().Input(5),
                        log = new Log().InsertLog(usuarioId)
                    };

                    listEmpresa.Add(empresa);
                }

                InsertList(listEmpresa);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Empresa, "", listEmpresa.Count(), string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return true;
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Empresa, string.Empty, string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return false;
            }
            
        }
    }
}
