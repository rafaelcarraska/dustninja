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
using DustMedicalNinja.Models.ViewModel;

namespace DustMedicalNinja.Business
{
    internal class TemplateImpressaoBusiness : Uteis
    {
        TemplateImpressaoDao _TemplateImpressaoDao = new TemplateImpressaoDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;

        internal TemplateImpressaoBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal IEnumerable<Combobox> ListCombo()
        {
            var result = _TemplateImpressaoDao.ListCombo(empresaId).Result;
            return from r in result
                   orderby r.descricao
                   select new Combobox()
                   {
                       Id = r.Id,
                       descricao = r.descricao
                   };
        }

        internal IEnumerable<Combobox> ListaComboByFacility(string facilityId)
        {
            var listaTemplateImpressao = new FacilityBusiness(_HttpContext).Lista(facilityId).listaTemplateImpressao;

            var result = _TemplateImpressaoDao.ListaComboByFacility(listaTemplateImpressao).Result;
            return from r in result
                   orderby r.status descending, r.descricao
                   select new Combobox
                   {
                       Id = r.Id,
                       descricao = r.descricao
                   };
        }

        internal Msg Insert(TemplateImpressao templateImpressao)
        {
            msg = new Msg();
            try
            {
                templateImpressao.empresaId = empresaId;
                templateImpressao.log = new Log().InsertLog(usuarioId);
                msg.id = _TemplateImpressaoDao.Insert(templateImpressao).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o template de impressão: {templateImpressao.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TemplateImpressao, templateImpressao, templateImpressao.Id, "Insert", erro);
            }
        }

        internal void InsertList(List<TemplateImpressao> listaTemplateImpressao)
        {
            if (listaTemplateImpressao.Count > 0)
            {
                _TemplateImpressaoDao.InsertList(listaTemplateImpressao);
            }
        }

        internal List<TemplateImpressao> ListAll()
        {
            return _TemplateImpressaoDao.ListAll(empresaId).Result;
        }

        internal TemplateImpressao List(string Id)
        {
            if (!string.IsNullOrEmpty(Id) && Id != "0") {
                List<TemplateImpressao> listaTemplateImpressao = _TemplateImpressaoDao.List(Id).Result;
                if (listaTemplateImpressao.Count > 0)
                {
                    return listaTemplateImpressao.First();
                }
            }
            return new TemplateImpressao();
        }

        internal TemplateImpressaoViewModel ListViewModel(string Id)
        {
            if (!string.IsNullOrEmpty(Id) && Id != "0")
            {
                var listaTemplateImpressao = _TemplateImpressaoDao.List(Id).Result;

                if (listaTemplateImpressao != null)
                {
                    var result = listaTemplateImpressao;

                    return (from r in listaTemplateImpressao
                            select new TemplateImpressaoViewModel()
                            {
                                Id = r.Id,
                                empresaId = r.empresaId,
                                log = r.log,
                                status = r.status,
                                descricao = r.descricao,
                                cabecalho = r.cabecalho,
                                repetiCabecalho = r.repetiCabecalho,
                                corpo = r.corpo,
                                rodape = r.rodape,
                                repetiRodape = r.repetiRodape,
                                headerHeight = r.headerHeight,
                                footerHeight = r.footerHeight,
                                countAnexo = new AnexoTemplatesBusiness(_HttpContext).CountAnexoTemplate(Id),
                            }).FirstOrDefault();
                }
            }
            return new TemplateImpressaoViewModel();
        }

        internal Msg Salvar(TemplateImpressao templateImpressao)
        {
            templateImpressao.empresaId = empresaId;
            msg = Validar(templateImpressao);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(templateImpressao.Id))
                {
                    return Insert(templateImpressao);
                }

                return Update(templateImpressao);
            }

            return msg;
        }

        internal Msg Update(TemplateImpressao templateImpressao)
        {
            msg = new Msg();
            try
            {
                templateImpressao.log = templateImpressao.log.UpdateLog(usuarioId);
                _TemplateImpressaoDao.Update(templateImpressao);
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o template de impressão: {templateImpressao.descricao}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TemplateImpressao, templateImpressao, templateImpressao.Id, "Update", erro);
            }
        }

        internal string TemplateImpresasaoUtilizado(string Id)
        {
            var fileDCM = new FileDCMBusiness(_HttpContext).Lista(Id);
            return fileDCM.TemplateLaudando();
        }

        internal Msg Validar(TemplateImpressao templateImpressao)
        {
            List<string> erros = new List<string>();

            if (!_TemplateImpressaoDao.ExisteDescricao(templateImpressao).Result.Equals(0))
                erros.Add("Essa descrição do template de impressão já existe!");

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Delete(string Id)
        {
            msg = new Msg();            
            try
            {
                _TemplateImpressaoDao.Delete(Id);
                return msg;
            }
            catch (Exception ex)
            {
                msg.erro = new List<string>();
                string erro = $"Erro ao deletar o template de impressão.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TemplateImpressao, string.Empty, Id, "Delete", erro);
            }            
        }

        internal bool Input(int quantidade)
        {
            try
            {
                var listaTemplateImpressao = new List<TemplateImpressao>();

                for (int x = 0; x < quantidade; x++)
                {
                    var templateImpressao = new TemplateImpressao
                    {
                        descricao = "Mascara " + Aleatorio(10, 2),
                        cabecalho = "Cabecalho" + Aleatorio(12, 50, true, true, true, true),
                        repetiCabecalho = true,
                        corpo = "Corpo" + Aleatorio(12, 50, true, true, true, true),
                        rodape = "Rodape" + Aleatorio(12, 50, true, true, true, true),
                        repetiRodape = true,
                        empresaId = empresaId,
                        status = (new Random().Next(0, 2) == 0),
                        log = new Log().InsertLog(usuarioId)
                    };

                    listaTemplateImpressao.Add(templateImpressao);
                }

                _TemplateImpressaoDao.InsertList(listaTemplateImpressao);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Empresa, "", listaTemplateImpressao.Count(), string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return true;
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.TemplateImpressao, string.Empty, string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return false;
            }

        }


    }
}
