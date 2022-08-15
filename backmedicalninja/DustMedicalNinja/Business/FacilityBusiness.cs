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
    internal class FacilityBusiness : Uteis
    {
        FacilityDao _FacilityDao = new FacilityDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;

        internal FacilityBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());
        }

        internal Msg Insert(Facility facility)
        {
            msg = new Msg();
            try
            {
                facility.empresaId = empresaId;
                facility.log = new Log().InsertLog(usuarioId);
                string rnd = "-" + (Aleatorio(20, 1, true, false, false)+"FSIWN").Substring(1, 5).ToUpper();
                string nomeFantasia = new EmpresaBusiness(_HttpContext).Lista(empresaId).nomeFantasia.ToUpper();
                facility.aeTitle = nomeFantasia + rnd.ToUpper();
                msg.id = _FacilityDao.Insert(facility).Result;
                return msg;
            }
            catch (Exception ex)
            {
                var erro = $"Erro ao editar a unidade: {facility.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Facility, facility, facility.Id, "Insert", erro);
            }
        }

        internal void InsertList(List<Facility> listFacility)
        {
            if (listFacility.Count > 0)
            {
                _FacilityDao.InsertList(listFacility);
            }
        }

        internal IEnumerable<Facility> ListAllAtiva()
        {
            return _FacilityDao.ListAllAtiva(empresaId).Result;           
        }

        internal IEnumerable<Facility> ListAll()
        {
            var result = _FacilityDao.ListAll(empresaId).Result;
            return from r in result
                   orderby r.status descending, r.descricao
                   select r;
        }

        internal IEnumerable<Facility> ListaByUsuario()
        {
            if (master)
            {
                return _FacilityDao.ListaByUsuarioEmpresaId(empresaId).Result;
            }
            return _FacilityDao.ListaByUsuario(empresaId, usuarioId).Result;
            
        }

        internal IEnumerable<Combobox> ListaCombo()
        {
            var result = _FacilityDao.ListaCombo(empresaId).Result;
            return from r in result
                   orderby r.status descending, r.descricao
                   select new Combobox
                   {
                       Id = r.Id,
                       descricao = r.descricao
                   };
        }

        internal IEnumerable<Combobox> ListaComboByUsuario(string usuarioId)
        {
            var listaEmpresaId = new UsuarioBusiness(_HttpContext).List(usuarioId).listaEmpresa;

            var result = _FacilityDao.ListaByUsuarioListaEmpresa(listaEmpresaId).Result;
            return from r in result
                   orderby r.status descending, r.descricao
                   select new Combobox
                   {
                       Id = r.Id,
                       descricao = r.descricao
                   };
        }

        internal long ListaFacilityCount(string empresaIdCount)
        {
            return _FacilityDao.ListaFacilityCount(empresaIdCount).Result;
            
        }

        internal Facility Lista(string Id)
        {
            List<Facility> list_fileDCM = _FacilityDao.List(Id).Result;
            if (list_fileDCM.Count > 0)
            {
                return list_fileDCM.First();
            }
            return new Facility();
        }

        internal Facility ListaEaTitle(string aeTitle)
        {
            List<Facility> list_fileDCM = _FacilityDao.ListaEaTitle(aeTitle).Result;
            if (list_fileDCM.Count > 0)
            {
                return list_fileDCM.First();
            }
            return new Facility();
        }

        internal Facility Lista()
        {
            List<Facility> list_fileDCM = _FacilityDao.List(empresaId).Result;
            if (list_fileDCM.Count > 0)
            {
                return list_fileDCM.First();
            }
            return new Facility();
        }

        internal Msg Salvar(Facility facility)
        {
            facility.empresaId = empresaId;
            msg = Validar(facility);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(facility.Id))
                {
                   return Insert(facility);
                }
                return Update(facility);
            }            

            return msg;
        }

        internal Msg Update(Facility facility)
        {
            msg = new Msg();
            try
            {
                facility.log = facility.log.UpdateLog(usuarioId);
                _FacilityDao.Update(facility);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar a unidade: {facility.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Facility, facility, facility.Id, "Update", erro);
            }
        }

        internal Msg Validar(Facility facility)
        {
            List<string> erros = new List<string>();

            if (facility.listaTemplateImpressao.Count <= 0)
            {
                erros.Add("É obrigatório selecionar um template de impressao!");
            }
            else if (!_FacilityDao.ExisteDescricao(facility).Result.Equals(0))
            {
                erros.Add("Essa descrição da unidade já existe!");
            }

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Delete(string Id)
        {
            msg = new Msg();            
            try
            {
                if (!EmUso(Id))
                {
                    _FacilityDao.Delete(Id);
                    new PermissaoBusiness(_HttpContext).DeletePermissaoFacility(Id);
                    return msg;
                }
                msg.erro = new List<string>();
                msg.erro.Add($"Não é possivel deletar essa unidade, pois ele está em uso.");
                //msg.erro.Add($"Status da unidade alterado para Inativo.");
                return msg;
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao deletar a facility.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Facility, "", Id, "Delete", erro);
            }
        }

        internal Prioridade PrioridadeFacility(string Id)
        {
            Prioridade prioridade = Lista(Id).prioridade;
            if (prioridade != null)
            {
                return prioridade;
            }
            return new Prioridade();
        }

        internal bool EmUso(string Id)
        {
            var aeTitle = Lista(Id).aeTitle;
            if (new FileDCMBusiness(_HttpContext).ExisteExames(aeTitle) > 0)
            {
                return true;
            }
            return false;
        }


        internal string Inativar(string Id)
        {
            msg = new Msg();
            try
            {
                Facility facility = Lista(Id);

                if (facility.status)
                {
                    facility.status = false;
                    Update(facility);
                }

                return $"Status da unidade alterado para Inativo.";
            }
            catch (Exception)
            {
                return $"Erro ao inativar a unidade.";
            }
        }

        internal bool Input(int quantidade)
        {
            try
            {
                var listFacility = new List<Facility>();

                for (int x = 0; x < quantidade; x++)
                {
                    var facility = new Facility
                    {
                        empresaId = empresaId,
                        descricao = "facility " + Aleatorio(10, 2),
                        tempoRetencaoImagens = new Random().Next(1, 365),
                        aeTitle = Aleatorio() + "@dust.ninja",
                        status = (new Random().Next(0, 2) == 0),
                        endereco = new EnderecoBusiness().Input(),
                        listaContato = new ContatoBusiness().Input(5),
                        prioridade = new PrioridadeBusiness().Input(),

                        log = new Log().InsertLog(usuarioId)
                    };

                    listFacility.Add(facility);
                }

                _FacilityDao.InsertList(listFacility);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Empresa, "", listFacility.Count(), string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return true;
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Facility, "Input", "", "quantidade solicitada: " + quantidade);
                return false;
            }

        }
    }
}
