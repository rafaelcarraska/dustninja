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
    internal class PermissaoBusiness : Uteis
    {
        PermissaoDao _PermissaoDao = new PermissaoDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;

        internal PermissaoBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());
        }

        internal Msg ProcessarBatchFacility(BatchPermissoesViewModel batchPermissoes)
        {
            msg = new Msg();
            try
            {
                PermissaoFacility _PermissaoFacility = new PermissaoFacility();
                _PermissaoFacility.facilityId = batchPermissoes.facilityId;
                List<Permissao> listaPermissao = new List<Permissao>();
                Permissao permissao = new Permissao();

                foreach (var usuario in batchPermissoes.listUsuarios)
                {
                    permissao = new Permissao();
                    permissao.usuarioId = usuario.Id;
                    permissao.facilityId = batchPermissoes.facilityId;
                    permissao.listaPermissao = batchPermissoes.listaPermissoes.Select(x => x.descricao).Distinct().ToList();
                    listaPermissao.Add(permissao);

                    DeletePermissaoFacilityUsuario(permissao.facilityId, permissao.usuarioId);
                }
                _PermissaoFacility.listaPermissao = listaPermissao;
               
                if (listaPermissao.Any())
                {
                    _PermissaoDao.Insert_List(_PermissaoFacility.listaPermissao);
                }

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao processar o batch de unidades.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Facility, batchPermissoes, batchPermissoes.facilityId, "Insert", erro);
            }
        }

        internal Msg ProcessarBatchUsuario(BatchPermissoesViewModel batchPermissoes)
        {
            msg = new Msg();
            try
            {
                PermissaoUsuario _PermissaoUsuario = new PermissaoUsuario();
                _PermissaoUsuario.usuarioId = batchPermissoes.usuarioId;
                List<Permissao> listaPermissao = new List<Permissao>();
                Permissao permissao = new Permissao();

                foreach (var facility in batchPermissoes.listUsuarios)
                {
                    permissao = new Permissao();
                    permissao.facilityId = facility.Id;
                    permissao.usuarioId = batchPermissoes.usuarioId;
                    permissao.listaPermissao = batchPermissoes.listaPermissoes.Select(x => x.descricao).Distinct().ToList();
                    listaPermissao.Add(permissao);

                    DeletePermissaoFacilityUsuario(permissao.facilityId, permissao.usuarioId);
                }
                _PermissaoUsuario.listaPermissao = listaPermissao;

                if (listaPermissao.Any())
                {
                    _PermissaoDao.Insert_List(_PermissaoUsuario.listaPermissao);
                }

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao processar o batch de unidades.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Facility, batchPermissoes, batchPermissoes.facilityId, "Insert", erro);
            }
        }

        internal Msg SalvarPermissaoFacility(PermissaoFacility permissaoFacility)
        {
            msg = new Msg();
            try
            {
                DeletePermissaoFacility(permissaoFacility.facilityId);
                if (permissaoFacility.listaPermissao.Any())
                {
                    permissaoFacility.listaPermissao.ForEach(x => x.listaPermissao = x.listaPermissao.Distinct().ToList());
                    _PermissaoDao.Insert_List(permissaoFacility.listaPermissao);
                }

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao salvar permissao da unidade.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Facility, permissaoFacility.listaPermissao, permissaoFacility.facilityId, "Insert", erro);
            }
        }

        internal Msg SalvarPermissaoUsuario(PermissaoUsuario permissaoUsuario)
        {
            msg = new Msg();
            try
            {
                DeletePermissaoUsuario(permissaoUsuario.usuarioId);
                if (permissaoUsuario.listaPermissao.Any())
                {
                    permissaoUsuario.listaPermissao.ForEach(x => x.listaPermissao = x.listaPermissao.Distinct().ToList());
                    _PermissaoDao.Insert_List(permissaoUsuario.listaPermissao);
                }

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao salvar permissao do usuário.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, permissaoUsuario.listaPermissao, permissaoUsuario.usuarioId, "Insert", erro);
            }

        }

        internal void DeletePermissaoFacility(string facilityId)
        {
            try
            {
                _PermissaoDao.Delete_FacilityId(facilityId);
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Facility, facilityId, facilityId, "Delete", "");
            }

        }

        internal void DeletePermissaoFacilityUsuario(string facilityId, string usuarioId)
        {
            try
            {
                _PermissaoDao.Delete_FacilityUsuario(facilityId, usuarioId);
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Facility, facilityId, usuarioId, "Delete", $"delete: facilityId {facilityId}, usuarioId {usuarioId}");
            }
        }

        internal void DeletePermissaoUsuario(string usuarioId)
        {
            try
            {
                _PermissaoDao.Delete_UsuarioId(usuarioId);
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Usuario, usuarioId, usuarioId, "Delete", "");
            }

        }

        internal IEnumerable<Permissao> List_FacilityId(string facilityId)
        {
            return _PermissaoDao.List_FacilityId(facilityId).Result;
        }

        internal IEnumerable<Permissao> List_UsuarioId(string usuarioId)
        {
            return _PermissaoDao.List_UsuarioId(usuarioId).Result;
        }

        internal IEnumerable<Permissao> List_Permissao_Workilist()
        {
            if (master)
            {
                return new List<Permissao>();
            }
            return List_UsuarioId(usuarioId);
        }

        internal bool verificarPermissao(List<Permissao> permissao, string facilityId, string value)
        {
            try
            {
                if (master)
                {
                    return true;
                }
                if (string.IsNullOrEmpty(facilityId) || string.IsNullOrEmpty(value) || permissao.Count() <= 0)
                {
                    return false;
                }
                return permissao.FirstOrDefault(x => x.facilityId == facilityId && x.usuarioId == usuarioId).listaPermissao.Contains(value);
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, permissao, facilityId, "Listar", "Erro ao verificar permissão evento: "+value);
                return false;
            }
            
        }
    }
}
