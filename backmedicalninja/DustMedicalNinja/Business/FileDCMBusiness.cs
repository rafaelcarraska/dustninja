using DustMedicalNinja.Components;
using DustMedicalNinja.Context;
using DustMedicalNinja.DAO;
using DustMedicalNinja.Extensions;
using DustMedicalNinja.Models;
using DustMedicalNinja.Models.Postgre;
using DustMedicalNinja.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DustMedicalNinja.Business
{
    internal class FileDCMBusiness : Uteis
    {
        FileDCMDao _FileDCMDao = new FileDCMDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;

        internal FileDCMBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal List<FileDCM> List_DCM4CHEE_NaoSinconizados(DCMContext _context)
        {
            try
            {
                SyncNinjaDao _SyncNinjaDao = new SyncNinjaDao();
                var facility = new FacilityBusiness(_HttpContext).ListAllAtiva();

                var list_study = (from study in _context.Study
                              .Include(t => t.series)
                              .Include(t => t.studyqueryattrs)
                              .Include(t => t.patient)
                                  join pat in _context.PersonName on study.patient.pat_name_fk equals pat.pk
                                  join patId in _context.PatientId on study.patient.patient_id_fk equals patId.pk
                                  where !(from sync in _SyncNinjaDao.ListAll().Result
                                          select sync.study_fk)
                                        .Contains(study.pk)
                                        &&
                                        (from fact in facility
                                         select fact.aeTitle)
                                        .Contains(study.series.FirstOrDefault(x => !string.IsNullOrEmpty(x.src_aet) && x.src_aet != "*").src_aet)
                                  select new FileDCM
                                  {
                                      pkPostgre = study.pk,
                                      studyId = study.study_iuid,
                                      studyDesc = study.study_desc,
                                      aeTitle = study.series.FirstOrDefault(x => !string.IsNullOrEmpty(x.src_aet) && x.src_aet != "*").src_aet,
                                      body_part = study.series.FirstOrDefault(x => !string.IsNullOrEmpty(x.body_part) && x.body_part != "*").body_part,
                                      institution = study.series.FirstOrDefault(x => !string.IsNullOrEmpty(x.institution) && x.institution != "*").institution,
                                      department = study.series.FirstOrDefault(x => !string.IsNullOrEmpty(x.department) && x.department != "*").department,
                                      modality = study.studyqueryattrs.FirstOrDefault(x => !string.IsNullOrEmpty(x.mods_in_study) && x.mods_in_study != "*").mods_in_study,
                                      series_desc = study.series.FirstOrDefault(x => !string.IsNullOrEmpty(x.series_desc) && x.series_desc != "*").series_desc,
                                      date_study = study.study_date.StringToDate(study.study_time),
                                      data_envio = study.created_time,
                                      pacienteId = new PacienteBusiness(_HttpContext).SalvarSync(study.patient, pat, patId),
                                      empresaId = empresaId,
                                      status = true,
                                      log = new Log().InsertLog(usuarioId),
                                      prioridade = TipoPrioridade.normal.ToString("g"),
                                      statusExames = StatusExames.transmissao.ToString("g"),
                                      subStatusExames = SubStatusExames.novo.ToString("g")
                                  }).Distinct().ToList();

                return list_study;
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, string.Empty, string.Empty, "Sincronismo", "");
                return new List<FileDCM>();
            }
        }

        //acontece que algumas vezes de ter alguma informacao no postgre, porem nao esta tudo completo. assim a modalidade fica em branco.
        //o metodo abaixo localiza esse exame com modalidade em branco e tenta corrigi
        private void CorrigiFileDCM(DCMContext _context)
        {
            var fileDCM = _FileDCMDao.ListModalidadeVazia().Result;

            SyncNinjaDao _SyncNinjaDao = new SyncNinjaDao();
            var syncNinja = _SyncNinjaDao.ListaSyncFileDCM(fileDCM.Select(x => x.Id).ToList()).Result;

            var list_study = (from studyqueryattrs in _context.StudyQueryAttrs
                              where (from sync in syncNinja
                                     select sync.study_fk)
                                    .Contains(studyqueryattrs.study_fk)
                              && !string.IsNullOrEmpty(studyqueryattrs.mods_in_study)
                              select studyqueryattrs).Distinct().ToList();

            foreach (var sync in syncNinja)
            {
                var study = list_study.Where(x => x.study_fk == sync.study_fk && !string.IsNullOrEmpty(x.mods_in_study));
                if (study.Count() > 0)
                {
                    var fileDCMUpdate = fileDCM.FirstOrDefault(x => x.Id == sync.filedcm);
                    fileDCMUpdate.modality = study.FirstOrDefault().mods_in_study;
                    Update(fileDCMUpdate);
                }
            }
        }

        internal List<FileDCMViewModel> List_DCM4CHEE(DCMContext _context)
        {
            SincronizaFileDCM(_context);

            return Carregar_DCM4CHEE();
        }

        internal List<FileDCMViewModel> List_DCM4CHEEFiltroPage(DCMContext _context, string filtro)
        {
            var tipo = filtro.Split(':');

            switch (tipo[0])
            {
                case "paciente":
                    return Carregar_DCM4CHEEFiltroPagePaciente(tipo[1]);
                default:
                    return Carregar_DCM4CHEE();
            }
        }

        internal List<FileDCMViewModel> List_DCM4CHEE_Filtro(string filtroId)
        {
            return Carregar_DCM4CHEEFiltro(filtroId);
        }

        internal List<FileDCMViewModel> Carregar_DCM4CHEE()
        {
            var fileDCM = _FileDCMDao.List_All(empresaId).Result;
            var paciente = new PacienteBusiness(_HttpContext).ListAll();
            var facility = new FacilityBusiness(_HttpContext).ListaByUsuario();
            var favoritos = new FavoritosBusiness(_HttpContext).List();
            var anexos = new AnexosBusiness(_HttpContext).ListaAnexoByFileDCMId(fileDCM.Select(x => x.Id).ToList());
            var usuarioCountNota = new UsuarioCountNotaBusiness(_HttpContext).ListUsuario();

            var permissoes = new PermissaoBusiness(_HttpContext).List_Permissao_Workilist().ToList();

            var fileDCMVM = (from file in fileDCM
                             join pact in paciente on file.pacienteId equals pact.Id
                             join fact in facility on file.aeTitle equals fact.aeTitle
                             select new FileDCMViewModel
                             {
                                 Id = file.Id,
                                 log = file.log,
                                 status = file.status,
                                 empresaId = file.empresaId,
                                 pkPostgre = file.pkPostgre,
                                 fileName = file.fileName,
                                 studyId = file.studyId,
                                 studyDesc = file.studyDesc,
                                 aeTitle = file.aeTitle,
                                 body_part = file.body_part,
                                 institution = file.institution,
                                 department = file.department,
                                 modality = file.modality,
                                 dateConfirmacao = file.dateConfirmacao,
                                 series_desc = file.series_desc,
                                 pendente = new NotasBusiness(_HttpContext).Existe(file.Id),
                                 favorito = favoritos.Select(x => x.filedcmId).Contains(file.Id),
                                 date_study = file.date_study,
                                 date_envio = file.data_envio,
                                 pacienteId = file.pacienteId,
                                 pacienteNome = pact.nomeCompleto,
                                 paciente = pact,
                                 facilityDesc = fact.descricao,
                                 facilityId = fact.Id,
                                 permitirDownload = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Download da imagem"),
                                 removerNotas = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Remover notas"),
                                 permitirLaudo = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Laudar imagem"),
                                 confirmarExame = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Confirmar exame"),
                                 countAnexo = new AnexosBusiness(_HttpContext).CountAnexoByList(anexos, file.Id),
                                 countNota = new UsuarioCountNotaBusiness(_HttpContext).CountNotaByUsuario(usuarioCountNota, file.Id),
                                 statusExames = new ExameBusiness(_HttpContext).verificaStatusExame(file.statusExames.StatusExamesConvert(), file),
                                 subStatusExames = file.subStatusExames.SubStatusExamesConvert(),
                                 prioridade = file.prioridade,
                                 tempoParaLaudar = CalculaTempoParaLaudar(file.dateConfirmacao, fact, file.prioridade, file.statusExames.StatusExamesConvert())
                             })
                             .OrderByDescending(x => x.date_envio)
                             .ToList();

            return fileDCMVM;
        }

        internal List<FileDCMViewModel> Carregar_DCM4CHEEFiltro(string filtroId)
        {
            var filtro = new FiltroBusiness(_HttpContext).List(filtroId);

            var fileDCM = _FileDCMDao.List_All(empresaId).Result;
            var paciente = new PacienteBusiness(_HttpContext).ListAll();
            var facility = new FacilityBusiness(_HttpContext).ListaByUsuario();
            var favoritos = new FavoritosBusiness(_HttpContext).List();
            var anexos = new AnexosBusiness(_HttpContext).ListaAnexoByFileDCMId(fileDCM.Select(x => x.Id).ToList());

            var usuarioCountNota = new UsuarioCountNotaBusiness(_HttpContext).ListUsuario();

            var permissoes = new PermissaoBusiness(_HttpContext).List_Permissao_Workilist().ToList();

            var fileDCMVM = (from file in fileDCM
                             join pact in paciente on file.pacienteId equals pact.Id
                             join fact in facility on file.aeTitle equals fact.aeTitle
                             select new FileDCMViewModel
                             {
                                 Id = file.Id,
                                 log = file.log,
                                 status = file.status,
                                 empresaId = file.empresaId,
                                 pkPostgre = file.pkPostgre,
                                 fileName = file.fileName,
                                 studyId = file.studyId,
                                 studyDesc = file.studyDesc,
                                 aeTitle = file.aeTitle,
                                 body_part = file.body_part,
                                 institution = file.institution,
                                 department = file.department,
                                 modality = file.modality,
                                 dateConfirmacao = file.dateConfirmacao,
                                 series_desc = file.series_desc,
                                 pendente = new NotasBusiness(_HttpContext).Existe(file.Id),
                                 favorito = favoritos.Select(x => x.filedcmId).Contains(file.Id),
                                 date_study = file.date_study,
                                 date_envio = file.data_envio,
                                 pacienteId = file.pacienteId,
                                 pacienteNome = pact.nomeCompleto,
                                 paciente = pact,
                                 facilityDesc = fact.descricao,
                                 facilityId = fact.Id,
                                 permitirDownload = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Download da imagem"),
                                 removerNotas = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Remover notas"),
                                 permitirLaudo = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Laudar imagem"),
                                 confirmarExame = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Confirmar exame"),
                                 //countAnexo = new AnexosBusiness(_HttpContext).CountAnexoByList(anexos, file.Id),
                                 //countNota = new UsuarioCountNotaBusiness(_HttpContext).CountNotaByUsuario(usuarioCountNota, file.Id),
                                 statusExames = new ExameBusiness(_HttpContext).verificaStatusExame(file.statusExames.StatusExamesConvert(), file),
                                 subStatusExames = file.subStatusExames.SubStatusExamesConvert(),
                                 prioridade = file.prioridade,
                                 tempoParaLaudar = CalculaTempoParaLaudar(file.dateConfirmacao, fact, file.prioridade, file.statusExames.StatusExamesConvert())
                             })                             
                             .Where(x => filtro.listaFiltroStatus.Select(y => y.id).Contains(x.statusExamesFormatado));

            var ordem = filtro.listaFiltroOrdem.FirstOrDefault();
            if (ordem != null)
            {
                switch (ordem.id)
                {
                    case "dataEnvio":
                        if (ordem.ordem == "Crescente")
                        {
                            return fileDCMVM.OrderBy(x => x.date_envio).ToList();
                        }
                        else
                        {
                            return fileDCMVM.OrderByDescending(x => x.date_envio).ToList();
                        }
                    case "dataExame":
                        if (ordem.ordem == "Crescente")
                        {
                            return fileDCMVM.OrderBy(x => x.date_study).ToList();
                        }
                        else
                        {
                            return fileDCMVM.OrderByDescending(x => x.date_study).ToList();
                        }
                    case "paciente":
                        if (ordem.ordem == "Crescente")
                        {
                            return fileDCMVM.OrderBy(x => x.pacienteNome).ToList();
                        }
                        else
                        {
                            return fileDCMVM.OrderByDescending(x => x.pacienteNome).ToList();
                        }
                    case "unidade":
                        if (ordem.ordem == "Crescente")
                        {
                            return fileDCMVM.OrderBy(x => x.facilityDesc).ToList();
                        }
                        else
                        {
                            return fileDCMVM.OrderByDescending(x => x.facilityDesc).ToList();
                        }
                    default:
                        return fileDCMVM.OrderByDescending(x => x.date_envio).ToList();                        
                }
            }
                             
            return fileDCMVM.ToList();
        }

        internal List<FileDCMViewModel> Carregar_DCM4CHEEFiltroPagePaciente(string pacienteId)
        {
            var fileDCM = _FileDCMDao.ListaPaciente(pacienteId).Result;
            var paciente = new PacienteBusiness(_HttpContext).Lista(pacienteId);
            var facility = new FacilityBusiness(_HttpContext).ListaByUsuario();
            var favoritos = new FavoritosBusiness(_HttpContext).List();
            var anexos = new AnexosBusiness(_HttpContext).ListaAnexoByFileDCMId(fileDCM.Select(x => x.Id).ToList());
            var usuarioCountNota = new UsuarioCountNotaBusiness(_HttpContext).ListUsuario();

            var permissoes = new PermissaoBusiness(_HttpContext).List_Permissao_Workilist().ToList();

            var fileDCMVM = (from file in fileDCM
                             join fact in facility on file.aeTitle equals fact.aeTitle
                             select new FileDCMViewModel
                             {
                                 Id = file.Id,
                                 log = file.log,
                                 status = file.status,
                                 empresaId = file.empresaId,
                                 pkPostgre = file.pkPostgre,
                                 fileName = file.fileName,
                                 studyId = file.studyId,
                                 studyDesc = file.studyDesc,
                                 aeTitle = file.aeTitle,
                                 body_part = file.body_part,
                                 institution = file.institution,
                                 department = file.department,
                                 modality = file.modality,
                                 dateConfirmacao = file.dateConfirmacao,
                                 series_desc = file.series_desc,
                                 pendente = new NotasBusiness(_HttpContext).Existe(file.Id),
                                 favorito = favoritos.Select(x => x.filedcmId).Contains(file.Id),
                                 date_study = file.date_study,
                                 date_envio = file.data_envio,
                                 pacienteId = file.pacienteId,
                                 pacienteNome = paciente.nomeCompleto,
                                 paciente = paciente,
                                 facilityDesc = fact.descricao,
                                 facilityId = fact.Id,
                                 permitirDownload = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Download da imagem"),
                                 removerNotas = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Remover notas"),
                                 permitirLaudo = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Laudar imagem"),
                                 confirmarExame = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, fact.Id, "Confirmar exame"),
                                 countAnexo = new AnexosBusiness(_HttpContext).CountAnexoByList(anexos, file.Id),
                                 countNota = new UsuarioCountNotaBusiness(_HttpContext).CountNotaByUsuario(usuarioCountNota, file.Id),
                                 statusExames = new ExameBusiness(_HttpContext).verificaStatusExame(file.statusExames.StatusExamesConvert(), file),
                                 subStatusExames = file.subStatusExames.SubStatusExamesConvert(),
                                 prioridade = file.prioridade,
                                 tempoParaLaudar = CalculaTempoParaLaudar(file.dateConfirmacao, fact, file.prioridade, file.statusExames.StatusExamesConvert())
                             })
                             .OrderByDescending(x => x.date_envio)
                             .ToList();

            return fileDCMVM;
        }

        internal FileDCMViewModel ListaExame(string Id)
        {
            var fileDCM = _FileDCMDao.List(Id).Result;
            var paciente = new PacienteBusiness(_HttpContext).Lista(fileDCM.pacienteId);
            var facility = new FacilityBusiness(_HttpContext).ListaEaTitle(fileDCM.aeTitle);
            var usuarioCountNota = new UsuarioCountNotaBusiness(_HttpContext).ListUsuario();

            var permissoes = new PermissaoBusiness(_HttpContext).List_Permissao_Workilist().ToList();

            var fileDCMVM = new FileDCMViewModel()
            {
                Id = fileDCM.Id,
                log = fileDCM.log,
                status = fileDCM.status,
                empresaId = fileDCM.empresaId,
                pkPostgre = fileDCM.pkPostgre,
                fileName = fileDCM.fileName,
                studyId = fileDCM.studyId,
                studyDesc = fileDCM.studyDesc,
                aeTitle = fileDCM.aeTitle,
                body_part = fileDCM.body_part,
                institution = fileDCM.institution,
                department = fileDCM.department,
                modality = fileDCM.modality,
                series_desc = fileDCM.series_desc,
                pendente = new NotasBusiness(_HttpContext).Existe(fileDCM.Id),
                date_study = fileDCM.date_study,
                date_envio = fileDCM.data_envio,
                pacienteId = fileDCM.pacienteId,
                pacienteNome = paciente.nomeCompleto,
                paciente = paciente,
                facilityDesc = facility.descricao,
                facilityId = facility.Id,
                notaRadiologista = facility.notaRadiologista,
                permitirDownload = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, facility.Id, "Download da imagem"),
                removerNotas = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, facility.Id, "Remover notas"),
                countAnexo = new AnexosBusiness(_HttpContext).CountAnexo(fileDCM.Id),
                countNota = new UsuarioCountNotaBusiness(_HttpContext).CountNotaByUsuario(usuarioCountNota, fileDCM.Id),
                statusExames = new ExameBusiness(_HttpContext).verificaStatusExame(fileDCM.statusExames.StatusExamesConvert(), fileDCM),
                subStatusExames = fileDCM.subStatusExames.SubStatusExamesConvert(),
                prioridade = fileDCM.prioridade,
                historicoExame = fileDCM.historicoExame
            };

            return fileDCMVM;
        }

        internal async void SincronizaFileDCM(DCMContext _context)
        {
            var webConfig = new WebConfigBusiness(_HttpContext).List();

            // O if abaixo evita sincronizacao seguidas(tempo definido no campo LimiteRefresh em minutos)
            // e sincronizacao paralelas o campo timeoutSincronizacao é o timeout da trava da sincornizacao, tbm em minutos
            //if (!webConfig.sincronizando || webConfig.dataLimiteSincronizacao < DateTime.Now)
            //TODO corrigir a data do server
            if (webConfig.dataLimiteRefresh < DateTime.Now && (!webConfig.sincronizando || webConfig.dataLimiteSincronizacao < DateTime.Now))
            {
                new WebConfigBusiness(_HttpContext).Update(webConfig, true);
                var listaSync = from sync in _FileDCMDao.Insert_List(List_DCM4CHEE_NaoSinconizados(_context))
                                select new SyncNinja
                                {
                                    study_fk = sync.pkPostgre,
                                    filedcm = sync.Id,
                                    empresaId = empresaId,
                                    status = true,
                                    log = new Log().InsertLog(usuarioId)
                                };

                SyncNinjaDao _SyncNinjaDao = new SyncNinjaDao();
                InsertList(listaSync.ToList());

                CorrigiFileDCM(_context);

                new WebConfigBusiness(_HttpContext).Update(webConfig, false);
            }
        }

        internal void InsertList(List<SyncNinja> list_syncNinja)
        {
            if (list_syncNinja.Any())
            {
                SyncNinjaDao _SyncNinjaDao = new SyncNinjaDao();
                _SyncNinjaDao.InsertList(list_syncNinja);
            }

        }

        internal long ExisteExames(string aeTitle)
        {
            return _FileDCMDao.ExisteExames(aeTitle).Result;
        }

        internal Msg Inserir(FileDCM fileDCM)
        {
            List<string> erros = new List<string>();
            fileDCM.empresaId = empresaId;
            msg = Validar(fileDCM);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(fileDCM.Id))
                {
                    msg = Insert(fileDCM);
                }
                else
                {
                    erros.Add("Erro ao validar o exame.");

                }

            }
            new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, msg, fileDCM, fileDCM.Id, $"Validação para add um exame");
            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Validar(FileDCM fileDCM)
        {
            List<string> erros = new List<string>();
            try
            {

            }
            catch (Exception ex)
            {
                string erro = "Erro ao validar o exame.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, fileDCM, fileDCM.Id, "Validar", erro);
            }
            new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, msg, fileDCM, fileDCM.Id, $"Validação de desmembrar exame");
            return new Msg() { erro = List_Erros(erros) };
        }

        private Msg Insert(FileDCM fileDCM)
        {
            msg = new Msg();
            try
            {
                fileDCM.empresaId = empresaId;
                fileDCM.log = new Log().InsertLog(usuarioId);
                msg.id = _FileDCMDao.Insert(fileDCM).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao add novo exame: {fileDCM.Id}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Filtros, fileDCM, fileDCM.Id, "Insert", erro);
            }
        }

        internal Msg Update(FileDCM fileDCM)
        {
            List<string> erros = new List<string>();
            try
            {
                fileDCM.log = fileDCM.log.UpdateLog(usuarioId);
                _FileDCMDao.Update(fileDCM);

                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, fileDCM, fileDCM.Id, "Exame Atualizado", string.Empty, false);
                return new Msg() { erro = List_Erros(erros) };
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao atualizar o exame.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, fileDCM, fileDCM.Id, "Update", erro);
            }
        }

        internal Msg UpdateSync(FileDCM fileDCM, DCMContext _context)
        {
            List<string> erros = new List<string>();
            try
            {
                fileDCM.log = fileDCM.log.UpdateLog(usuarioId);
                _FileDCMDao.UpdateSync(fileDCM);
                var pkPostgre = new PacienteBusiness(_HttpContext).Lista(fileDCM.pacienteId).pkPostgre;               
                
                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, fileDCM, fileDCM.Id, "Exame Atualizado", string.Empty, false);

                var patient = _context.Patient.FirstOrDefault(x => x.pk == pkPostgre);
                var study = _context.Study.FirstOrDefault(x => x.pk == fileDCM.pkPostgre);
                study.patient = patient;
                _context.SaveChanges();

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao atualizar o exame.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, fileDCM, fileDCM.Id, "Update", erro);
            }
        }

        internal FileDCM Lista(string Id)
        {
            return _FileDCMDao.List(Id).Result;
        }

        internal TemplateImpressao BuscaParemetros(string templateImpresasaoUtilizado, string fileDcmId)
        {
            var temmplateImpressao = new TemplateImpressaoBusiness(_HttpContext).List(templateImpresasaoUtilizado);
            var exame = ListaExame(fileDcmId);
            var confimacao = exame.historicoExame.ListaStatus(StatusExames.laudar);
            var laudado = exame.historicoExame.ListaStatus(StatusExames.laudado);

            var usuarioLaudoId = laudado.log.insertUsuarioId;
            var usuarioLaudo = new UsuarioBusiness(_HttpContext).List(usuarioLaudoId);
            var usuario2LaudoId = laudado.segundaLeitura?.insertUsuarioId;
            var usuario2Laudo = usuario2LaudoId != null && usuario2LaudoId != usuarioLaudoId ? new UsuarioBusiness(_HttpContext).List(usuario2LaudoId): null;

            temmplateImpressao.corpo = SubstitucaoParametros(temmplateImpressao.corpo, exame, confimacao, laudado, usuarioLaudo, usuario2Laudo);
            temmplateImpressao.cabecalho = SubstitucaoParametros(temmplateImpressao.cabecalho, exame, confimacao, laudado, usuarioLaudo, usuario2Laudo);
            temmplateImpressao.rodape = SubstitucaoParametros(temmplateImpressao.rodape, exame, confimacao, laudado, usuarioLaudo, usuario2Laudo);           

            return temmplateImpressao;
        }

        public string SubstitucaoParametros(string texto, FileDCMViewModel exame, Confirmacao confimacao, Confirmacao laudado, Usuario usuarioLaudo, Usuario usuario2Laudo)
        {
            texto = texto.Replace("]]", "").Replace("[[", "$");

            texto = texto.Replace("$Nome", exame.pacienteNome);
            texto = texto.Replace("$Data_Nasc", exame.paciente.dataNascimento_formatada);
            texto = texto.Replace("$ID", exame.paciente.pacienteIdDCM);
            texto = texto.Replace("$Sexo", exame.paciente.sexo.SexoToString());
            texto = texto.Replace("$Idade", exame.paciente.dataNascimento.Idade().ToString());
            texto = texto.Replace("$Data_Hora", DateTime.Now.ToString("dd/MM/yyyy mm:hh"));
            texto = texto.Replace("$Data", DateTime.Now.ToString("dd/MM/yyyy"));
            texto = texto.Replace("$Hora", DateTime.Now.ToString("mm:hh"));
            texto = texto.Replace("$Modalidade", exame.modality);
            texto = texto.Replace("$Lateralidade", exame.body_part);
            texto = texto.Replace("$Descricao", exame.studyDesc);
            texto = texto.Replace("$Body_Part", exame.body_part);
            texto = texto.Replace("$DICOM_Institution", exame.institution);
            texto = texto.Replace("$Data_Hora_Confirmação", confimacao.log.updateDataEncerramento ?? "N/A"); //TODO BUSCAR CAMPO CERTO
            texto = texto.Replace("$Data_Hora_Recebimento", exame.date_study_formatada);//TODO BUSCAR CAMPO CERTO
            texto = texto.Replace("$Nome_Unidade", exame.facilityDesc);//TODO BUSCAR CAMPO CERTO
            texto = texto.Replace("$RazaoSocial_Unidade", exame.facilityDesc);//TODO BUSCAR CAMPO CERTO
            texto = texto.Replace("$Nome_Empresa", exame.facilityDesc);//TODO BUSCAR CAMPO CERTO
            texto = texto.Replace("$Laudo", laudado.historiaClinica);
            texto = texto.Replace("$LINHA1_MEDICO1", usuarioLaudo.assinatura.linha1);
            texto = texto.Replace("$LINHA2_MEDICO1", usuarioLaudo.assinatura.linha2);
            texto = texto.Replace("$LINHA3_MEDICO1", usuarioLaudo.assinatura.linha3);          
            texto = texto.Replace("$DATA_ASSINATURA", laudado.log.updateDataEncerramento);
            texto = texto.Replace("$IMAGEM_ASSINATURA1", $"<img style=\"clear: both; max-height: 90px !important;\" src='{Startup.urlFont}/assets/images/signature/{usuarioLaudo.assinatura.arquivo}'>");

            if (usuario2Laudo != null)
            {
                texto = texto.Replace("$LINHA1_MEDICO2", usuario2Laudo.assinatura.linha1);
                texto = texto.Replace("$LINHA2_MEDICO2", usuario2Laudo.assinatura.linha2);
                texto = texto.Replace("$LINHA3_MEDICO2", usuario2Laudo.assinatura.linha3);
                texto = texto.Replace("$IMAGEM_ASSINATURA2", $"<img style=\"clear: both; max-height: 90px !important;\" src='{Startup.urlFont}/assets/images/signature/{usuario2Laudo.assinatura.arquivo}'>");//TODO BUSCAR CAMPO CERTO

            }
            else
            {
                texto = texto.Replace("$LINHA1_MEDICO2", string.Empty);
                texto = texto.Replace("$LINHA2_MEDICO2", string.Empty);
                texto = texto.Replace("$LINHA3_MEDICO2", string.Empty);
                texto = texto.Replace("$IMAGEM_ASSINATURA2", string.Empty);
            }


            return texto;
        }

        internal LaudoPageViewModel GetLaudoPage(string fileDCMId, string templateImpressaoId = "")
        {
            if (string.IsNullOrEmpty(templateImpressaoId))
            {
                templateImpressaoId = new TemplateImpressaoBusiness(_HttpContext).TemplateImpresasaoUtilizado(fileDCMId);
            }

            var laudoPage = new LaudoPageViewModel();

            if (!string.IsNullOrEmpty(templateImpressaoId))
            {
                var temmplateImpressao = BuscaParemetros(templateImpressaoId, fileDCMId);

                laudoPage.conteudo = temmplateImpressao.corpo;
                laudoPage.header = temmplateImpressao.cabecalho;
                laudoPage.footer = temmplateImpressao.rodape;
                laudoPage.repetiCabecalho = temmplateImpressao.repetiCabecalho;
                laudoPage.repetiRodape = temmplateImpressao.repetiRodape;
                laudoPage.headerHeight = temmplateImpressao.headerHeight;
                laudoPage.footerHeight = temmplateImpressao.footerHeight;

            }

            return laudoPage;
        }

        internal long CountPacienteId(string pacienteId)
        {
            return _FileDCMDao.ListaPacienteCount(pacienteId).Result;
        }

        internal IEnumerable<PacienteCountExamesViewModel> CountPacienteExames(string empresaId)
        {
            return _FileDCMDao.CountPacienteExames(empresaId).Result;
        }

        internal string CalculaTempoParaLaudar(DateTime dateConfirmacao, Facility facility, string prioridade, StatusExames statusExames)
        {
            if (dateConfirmacao != null && dateConfirmacao > DateTime.MinValue && (statusExames == StatusExames.laudar || statusExames == StatusExames.laudando))
            {
                var tempoParaLaudo = 0;

                switch (prioridade)
                {
                    case "normal":
                        tempoParaLaudo = facility.prioridade.rotina;
                        break;
                    case "urgente":
                        tempoParaLaudo = facility.prioridade.urgencia;
                        break;
                    case "critico":
                        tempoParaLaudo = facility.prioridade.critico;
                        break;
                    default:
                        tempoParaLaudo = 0;
                        break;
                }

                var tempoRestante = tempoParaLaudo - (DateTime.Now - dateConfirmacao).TotalHours;


                return tempoRestante.doubleToTime();
            }

            return string.Empty;
        }

    }
}
