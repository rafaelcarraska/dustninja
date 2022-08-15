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
    internal class UsuarioBusiness : Uteis
    {
        UsuarioDao _UsuarioDao = new UsuarioDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;


        internal UsuarioBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());

        }

        internal Msg Insert(Usuario usuario)
        {
            msg = new Msg();
            try
            {
                usuario.log.InsertLog(usuarioId);

                if (usuario.twofactor)
                {
                    usuario.key = new AutenticacaoBusiness(_HttpContext).getMD5Hash(DateTime.Now.ToString() + usuario.senha);
                }

                msg.id = _UsuarioDao.Insert(usuario).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o usuário: {usuario.nome}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, usuario, usuario.Id, "Insert", erro);
            }
        }

        internal Msg NovoTwoFactor(string usuarioId)
        {
            msg = new Msg();
            try
            {
                var usuario = List(usuarioId);

                if (usuario.twofactor)
                {
                    usuario.key = new AutenticacaoBusiness(_HttpContext).getMD5Hash(DateTime.Now.ToString() + usuario.senha);
                }

                msg = Update(usuario);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar atualizar two factor key do usuarioId: {usuarioId}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, usuarioId, usuarioId, "Update", erro);
            }
        }

        internal void InsertList(List<Usuario> listUsuario)
        {
            if (listUsuario.Count > 0)
            {
                _UsuarioDao.InsertList(listUsuario);
            }
        }

        internal long CountPerfilId(string perfilId)
        {
            return _UsuarioDao.CountPerfilId(perfilId).Result;
        }

        internal long EmpresaEmUso(string empresaId)
        {
            return _UsuarioDao.EmpresaEmUso(empresaId).Result;
        }

        internal IEnumerable<Usuario> ListAll()
        {
            var result = new List<Usuario>();
            if (master)
            {
                result = _UsuarioDao.ListAllMaster().Result;
            }
            else
            {
                result = _UsuarioDao.ListAll().Result;
                var listaEmpresa = ListUsuarioLogado().listaEmpresa;
                result = result.Where(x => x.listaEmpresa.Count(z => ListUsuarioLogado().listaEmpresa.Contains(z)) > 0).ToList();
            }

            return from r in result
                   orderby r.status descending, r.nome
                   select r;
        }

        internal IEnumerable<Combobox> ListaCombo()
        {
            var query = UsuarioView();

            if (!master)
            {
                var listaEmpresa = ListUsuarioLogado().listaEmpresa;
                query = query.Where(x => x.listaEmpresa.Count(z => ListUsuarioLogado().listaEmpresa.Contains(z)) > 0).ToList();


            }
            return from r in query
                   orderby r.nome
                   select new Combobox()
                   {
                       Id = r.Id,
                       descricao = r.nome,
                       filtro1 = r.perfil,
                       filtroId1 = r.perfilId,
                       dataInclusao = r.dataInclusao,
                   };
        }

        internal List<UsuarioViewModel> UsuarioView()
        {
            var usuario = _UsuarioDao.ListaCombo().Result;
            var perfil = new PerfilBusiness(_HttpContext).ListCombo();

            return (from u in usuario
                    join p in perfil on u.perfilId equals p.Id
                    select new UsuarioViewModel()
                    {
                        Id = u.Id,
                        nome = u.nome,
                        perfil = p.descricao,
                        perfilId = p.Id,
                        dataInclusao = u.log.insertData,
                        listaEmpresa = u.listaEmpresa
                    }).ToList();
        }

        internal UsuarioViewModel UsuarioView(string usuarioId)
        {
            var usuario = List(usuarioId);
            var perfil = new PerfilBusiness(_HttpContext).Lista(usuario.perfilId);

            return new UsuarioViewModel()
            {
                Id = usuario.Id,
                nome = usuario.nome,
                perfil = perfil.descricao,
                perfilId = perfil.Id,
                dataInclusao = usuario.log.insertData,
                listaEmpresa = usuario.listaEmpresa
            };
        }

        internal Usuario List(string Id)
        {
            Usuario usuario = _UsuarioDao.List(Id).Result;
            if (usuario != null)
            {
                return usuario;
            }
            return new Usuario();
        }

        internal Usuario ListUsuarioLogado()
        {
            Usuario usuario = _UsuarioDao.List(usuarioId).Result;
            if (usuario != null)
            {
                return usuario;
            }
            return new Usuario();
        }

        internal Msg Salvar(Usuario usuario)
        {
            if (!string.IsNullOrEmpty(usuario.senha))
            {
                usuario.senha = new AutenticacaoBusiness().getMD5Hash(usuario.senha);
            }

            msg = Validar(usuario);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(usuario.Id))
                {
                    return Insert(usuario);
                }

                return Update(usuario);
            }

            return msg;
        }

        internal Msg Update(Usuario usuario)
        {
            msg = new Msg();
            try
            {
                usuario.log.UpdateLog(usuarioId);

                if (string.IsNullOrEmpty(usuario.senha))
                {
                    usuario.senha = List(usuario.Id).senha;
                }

                if (usuario.twofactor && string.IsNullOrEmpty(usuario.key))
                {
                    usuario.key = new AutenticacaoBusiness(_HttpContext).getMD5Hash(DateTime.Now.ToString() + usuario.senha);
                }
                if (!usuario.twofactor)
                {
                    usuario.key = string.Empty;
                }

                _UsuarioDao.Update(usuario);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o usuário: {usuario.nome}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, usuario, usuario.Id, "Update", erro);
            }
        }

        internal Msg Validar(Usuario usuario)
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(usuario.Id) && string.IsNullOrEmpty(usuario.senha))
            {
                msg.erro.Add($"O campo senha é obrigatório.");
            }
            if (!_UsuarioDao.ExisteNome(usuario).Result.Equals(0))
            {
                erros.Add("Essa nome de usuário já existe!");
            }
            if (!_UsuarioDao.ExisteLogin(usuario).Result.Equals(0))
            {
                erros.Add("Esse login já existe!");
            }
            if (string.IsNullOrEmpty(usuario.Id) && !master)// se for insert e o usuario nao for master, adicionar apenas a empresa logada
            {
                usuario.listaEmpresa.Add(empresaId);
            }
            if (!string.IsNullOrEmpty(usuario.Id) && !master)//se for update e o usuario nao for master, nao pode alterar a lista de empresa
            {
                if (!usuario.listaEmpresa.SequenceEqual(List(usuario.Id).listaEmpresa))
                {
                    erros.Add("Apenas usuário master pode adicionar empresas ao usuário!");
                }
            }
            if (!usuario.listaEmpresa.Any())
            {
                erros.Add("O usuário deve conter ao menos uma empresa!");
            }
            if (!master)//Apenas usuario master poder add ou alterar um usuario para master
            {
                if (string.IsNullOrEmpty(usuario.Id))
                {
                    if (usuario.master)
                    {
                        erros.Add("Apenas usuário master pode adicionar usuários master!");
                    }
                }
                else
                {
                    if (usuario.master != List(usuario.Id).master)
                    {
                        erros.Add("Apenas usuário master pode alterar usuários master!");
                    }
                }

            }

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Delete(string Id)
        {
            msg = new Msg();
            try
            {
                _UsuarioDao.Delete(Id);
                new PermissaoBusiness(_HttpContext).DeletePermissaoUsuario(Id);
                return msg;
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao deletar o usuário.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, "", Id, "Delete", erro);
            }
        }

        internal bool Input(int quantidade)
        {
            try
            {
                var listUsuario = new List<Usuario>();

                for (int x = 0; x < quantidade; x++)
                {
                    var usuario = new Usuario
                    {
                        nome = Aleatorio(10, 2),
                        login = "admin" + Aleatorio(x).TrimEnd(),
                        senha = new AutenticacaoBusiness().getMD5Hash("123"),
                        email = Aleatorio() + "@dust.ninja",
                        perfilId = new PerfilBusiness(_HttpContext).ListAll()
                            .OrderBy(r => new Random().Next()).Select(p => p.Id).FirstOrDefault(),
                        status = (new Random().Next(0, 2) == 0),
                        master = (new Random().Next(0, 2) == 0),
                        endereco = new EnderecoBusiness().Input(),
                        listaContato = new ContatoBusiness().Input(5),
                        assinatura = new AssinaturaBusiness().Input(),
                        log = new Log().InsertLog(usuarioId),
                        listaEmpresa = new EmpresaBusiness(_HttpContext).ListAll().Select(e => e.Id).ToList()
                    };

                    listUsuario.Add(usuario);
                }

                _UsuarioDao.InsertList(listUsuario);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Empresa, "", listUsuario.Count(), string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return true;
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, "Input",  "", "quantidade solicitada: " + quantidade);
                return false;
            }

        }
    }
}
