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
    internal class PerfilBusiness : Uteis
    {
        PerfilDao _PerfilDao = new PerfilDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string usuarioId;

        internal PerfilBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }        

        internal Msg Insert(Perfil perfil)
        {
            msg = new Msg();
            try
            {
                perfil.log = new Log().InsertLog(usuarioId);
                msg.id = _PerfilDao.Insert(perfil).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro  = $"Erro ao editar o perfil: {perfil.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Perfil, perfil, perfil.Id, "Insert", erro);
            }
        }

        internal void InsertList(List<Perfil> listPerfil)
        {
            if (listPerfil.Count > 0)
            {
                _PerfilDao.InsertList(listPerfil);
            }
        }

        internal IEnumerable<Perfil> ListAll()
        {
            var result = _PerfilDao.ListAll().Result;
            return from r in result
                   orderby r.status descending, r.descricao
                   select r;
        }

        internal IEnumerable<Combobox> ListCombo()
        {
            var result = _PerfilDao.ListCombo().Result;
            return from r in result
                   orderby r.descricao
                   select new Combobox() {
                       Id = r.Id,
                       descricao = r.descricao
                   };
        }

        internal Perfil Lista(string Id)
        {
            List<Perfil> listaPerfil = _PerfilDao.List(Id).Result;
            if (listaPerfil.Count > 0)
            {
                return listaPerfil.First();
            }
            return new Perfil();
        }

        internal List<string> Roles(string perfilId, bool master)
        {
            var perfil = Lista(perfilId);

            List<string> roles = new List<string>();
            foreach (var telas in perfil.acesso.listaTela)
            {
                foreach (var permissao in telas.permissao)
                {
                    roles.Add(telas.descricao + "_" + permissao);
                }
            }
            if (master)
            {
                roles.Add("Master");
            }

            return roles;
        }

        internal Msg Salvar(Perfil perfil)
        {
            msg = Validar(perfil);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(perfil.Id))
                {
                    return Insert(perfil);
                }

                return Update(perfil);
            }

            return msg;
        }

        internal Msg Update(Perfil perfil)
        {
            msg = new Msg();
            try
            {
                perfil.log = perfil.log.UpdateLog(usuarioId);
                _PerfilDao.Update(perfil);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o perfil: {perfil.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Perfil, perfil, perfil.Id, "Update", erro);
            }            
        }

        internal Msg Validar(Perfil perfil)
        {
            List<string> erros = new List<string>();

            if (!_PerfilDao.ExisteDescricao(perfil).Result.Equals(0))
                erros.Add("Essa descrição perfil já existe!");
            
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
                    _PerfilDao.Delete(Id);
                    return msg;
                }
                var msgInativar = Inativar(Id);
                if (string.IsNullOrEmpty(msgInativar))
                {
                    msg.erro.Add($"Não é possivel deletar esse perfil, pois ele está em uso.");
                    msg.erro.Add($"Status do perfil alterado para Inativo.");
                }
                listaErro.Add(msgInativar);

                msg.erro = listaErro;
                msg.status = false;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao deletar o perfil.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Perfil, string.Empty, Id, "Delete", erro);
            }            
        }

        internal string Inativar(string Id)
        {
            try
            {
                Perfil perfil = Lista(Id);

                if (perfil.status)
                {
                    perfil.status = false;
                    Update(perfil);
                }               

                return null;
            }
            catch (Exception)
            {
               return $"Erro ao inativar o perfil.";
            }
        }

        internal bool EmUso(string Id)
        {
            if (new UsuarioBusiness(_HttpContext).CountPerfilId(Id) > 0)
            {
                return true;
            }
            return false;
        }

        internal bool Input(int quantidade)
        {
            try
            {
                var listPerfil = new List<Perfil>();

                for (int x = 0; x < quantidade; x++)
                {
                    var perfil = new Perfil
                    {
                        descricao = "perfil" + Aleatorio(10, 2),
                        status = (new Random().Next(0, 2) == 0),
                        acesso = new AcessoBusiness(_HttpContext).Aleatorio(),
                        log = new Log().InsertLog(usuarioId)
                    };

                    listPerfil.Add(perfil);
                }

                _PerfilDao.InsertList(listPerfil);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Empresa, "", listPerfil.Count(), string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return true;
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Perfil, string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return false;
            }

        }


    }
}
