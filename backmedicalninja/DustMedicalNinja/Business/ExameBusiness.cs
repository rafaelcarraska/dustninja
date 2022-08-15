using DustMedicalNinja.Components;
using DustMedicalNinja.Context;
using DustMedicalNinja.DAO;
using DustMedicalNinja.Extensions;
using DustMedicalNinja.Models;
using DustMedicalNinja.Models.Postgre;
using DustMedicalNinja.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DustMedicalNinja.Business
{
    internal class ExameBusiness : Uteis
    {
        FileDCMDao _FileDCMDao = new FileDCMDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;

        internal ExameBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());
        }

        internal StatusExames verificaStatusExame(StatusExames statusExames, FileDCM fileDCM)
        {
            switch (statusExames)
            {
                case StatusExames.transmissao:
                    if (DateTime.Now > fileDCM.data_envio.AddMinutes(2))
                    {
                        return StatusExames.confirmar;
                    }
                    return statusExames;
                case StatusExames.laudando:
                    if (DateTime.Now > fileDCM.historicoExame.DataUltimoHistorico(fileDCM.log.updateData).AddMinutes(30))
                    {
                        return StatusExames.laudar;
                    }
                    return statusExames;
                default:
                    return statusExames;
            }

        }

        internal List<StatusExameViewModel> ListaStatus()
        {
            try
            {
                var _statusExames = Enum.GetValues(typeof(StatusExames)).Cast<StatusExames>();
                List<StatusExameViewModel> listStatusExames = new List<StatusExameViewModel>();
                foreach (var item in _statusExames)
                {
                    listStatusExames.Add(new StatusExameViewModel()
                    {
                        statusExames = item
                    });
                }
                return listStatusExames;
            }
            catch (Exception)
            {
                return new List<StatusExameViewModel>();
            }
        }

        internal StatusExameViewModel verificaStatusExame(string fileDCMId)
        {
            FileDCM fileDCM = new FileDCMBusiness(_HttpContext).Lista(fileDCMId);
            if (fileDCM != null)
            {
                return new StatusExameViewModel()
                {
                    date_envio = fileDCM.data_envio,
                    statusExames = verificaStatusExame(fileDCM.statusExames.StatusExamesConvert(), fileDCM),
                    subStatusExames = fileDCM.subStatusExames.SubStatusExamesConvert(),
                    prioridade = fileDCM.prioridade,
                    usuarioLogado = fileDCM.UsuarioLaudando(usuarioId)
                };
            }

            return null;
        }

        internal string ConfirmacaoStatusExame(string fileDCMId)
        {
            try
            {
                fileDCMId = fileDCMId.Trim().Replace("'", "''");
                if (string.IsNullOrEmpty(fileDCMId) && (fileDCMId.Length > 50 || fileDCMId.Length < 15))
                {
                    return "Código inválido.";
                }
                FileDCM fileDCM = new FileDCMBusiness(_HttpContext).Lista(fileDCMId);
                if (fileDCM != null)
                {
                    return $"Status do exame: {verificaStatusExame(fileDCM.statusExames.StatusExamesConvert(), fileDCM).ToString()}";
                }

                return "Não possível validar o código do exame.";
            }
            catch (Exception)
            {
                return "Não possível validar o código do exame.";
            }
           
        }        

        internal ExameInfoViewModel ListaExameInfo(string fileDCMId, StatusExames status)
        {
            FileDCM fileDCM = new FileDCMBusiness(_HttpContext).Lista(fileDCMId);
            var confirmacao = fileDCM.historicoExame.ListaStatus(status);
            if (fileDCM != null && confirmacao != null)
            {
                var usuario = new UsuarioBusiness(_HttpContext).List(confirmacao.log.insertUsuarioId);
                return new ExameInfoViewModel()
                {
                    fileDCMId = fileDCM.fileName,
                    data = confirmacao.log.insertDataFormatada,
                    usuario = usuario.nome,
                    usuarioId = usuario.Id,
                    login = usuario.login
                };
            }

            return null;
        }

        internal bool VerificaRevisao(string fileDCMId)
        {
            var listaExameInfo = ListaExameInfo(fileDCMId, StatusExames.laudado);
            if (listaExameInfo == null)
            {
                return false;
            }
            return listaExameInfo.usuarioId != usuarioId;
        }

        internal Confirmacao CarregaUltimoLaudo(string fileDCMId)
        {
            try
            {
                var exame = new FileDCMBusiness(_HttpContext).ListaExame(fileDCMId);

                return exame.historicoExame.ListaStatus(StatusExames.laudado);
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao validar o exame.";
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, string.Empty, fileDCMId, "carregar ultimo laudo", erro);
                return new Confirmacao();
            }
        }

        internal Msg Salvar(FileDCM fileDCM, DCMContext _context)
        {
            msg = Validar(fileDCM);
            if (msg.erro == null)
            {
                return Update(fileDCM, _context);
            }

            return msg;
        }

        internal Msg Update(FileDCM fileDCM, DCMContext _context = null)
        {
            msg = new Msg();
            try
            {
                var fileDCMNovo = new FileDCMBusiness(_HttpContext).Lista(fileDCM.Id);
                fileDCMNovo.studyDesc = fileDCM.studyDesc;
                fileDCMNovo.body_part = fileDCM.body_part;
                fileDCMNovo.modality = fileDCM.modality;
                fileDCMNovo.log = fileDCM.log.UpdateLog(usuarioId);

                if (master)
                {
                    fileDCMNovo.status = fileDCM.status;
                }

                _FileDCMDao.Update(fileDCMNovo);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, fileDCMNovo, fileDCM, fileDCM.Id, $"Exame Editado");
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o exame: {fileDCM.studyDesc}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, fileDCM, fileDCM.Id, "Update", erro);
            }
        }

        internal Msg Validar(FileDCM fileDCM)
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(fileDCM.studyDesc))
            {
                erros.Add("O campo descrição é obrigatório!");
            }

            if (string.IsNullOrEmpty(fileDCM.body_part))
            {
                erros.Add("O campo estudo é obrigatório!");
            }

            if (string.IsNullOrEmpty(fileDCM.modality))
            {
                erros.Add("O campo modalidade é obrigatório!");
            }

            if (!fileDCM.status || !master)
            {
                erros.Add("Aceso negado.!");
            }

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Desmembrar(DesmembrarViewModel desmembrar)
        {
            List<string> erros = new List<string>();
            desmembrar.novosExames = desmembrar.novosExames.Where(x => x.Trim() != "").ToList();
            var fileDCM = new FileDCMBusiness(_HttpContext).Lista(desmembrar.fileDCMId);
            msg = ValidarDesmembrar(desmembrar, fileDCM);
            if (msg.erro == null)
            {
                var studyDescPai = fileDCM.studyDesc;
                fileDCM.studyDesc = desmembrar.novaDescricao;
                //atualizar o postgre
               
                msg = new FileDCMBusiness(_HttpContext).Update(fileDCM);
                if (msg.erro == null)
                {
                    var historicofileDCMPai = fileDCM.historicoExame;
                    
                    foreach (var novosExames in desmembrar.novosExames)
                    {
                        fileDCM.studyDesc = novosExames;
                        fileDCM.Id = null;
                        fileDCM.log = new Log();
                        fileDCM.fileDCMPai = desmembrar.fileDCMId;
                        
                        msg = new FileDCMBusiness(_HttpContext).Inserir(fileDCM);

                        if (desmembrar.confirmacao)
                        {
                            fileDCM.dateConfirmacao = DateTime.Now;
                            fileDCM.historicoExame = historicofileDCMPai;
                            fileDCM.statusExames = StatusExames.laudar.ToString("g");
                            fileDCM.subStatusExames = SubStatusExames.novo.ToString("g");
                        }
                        else
                        {
                            fileDCM.dateConfirmacao = new DateTime();
                            fileDCM.historicoExame = null;
                            fileDCM.statusExames = StatusExames.transmissao.ToString("g");
                            fileDCM.subStatusExames = SubStatusExames.novo.ToString("g");
                        }

                        if (msg.erro != null)
                        {
                            erros.AddRange(msg.erro);
                        }
                    }

                    new DesmembrarBusiness(_HttpContext).InsertAi(desmembrar, studyDescPai);
                }
            }
            return msg;
        }

        internal Msg ValidarDesmembrar(DesmembrarViewModel desmembrar, FileDCM fileDCM)
        {
            List<string> erros = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(desmembrar.fileDCMId))
                {
                    erros.Add($"Erro ao validar o exame.");
                }
                else
                {
                    if (fileDCM == null)
                    {
                        erros.Add("Erro ao validar o exame, Exame não localizado.");
                    }
                }
                if (string.IsNullOrEmpty(desmembrar.novaDescricao))
                {
                    erros.Add($"O campo descrição do exame base não pode ser vazio.");
                }
                if (desmembrar.novosExames.Count <= 0)
                {
                    erros.Add($"Não possui nenhuma descrição para novos exames.");
                }
            }
            catch (Exception ex)
            {
                string erro = "Erro ao validar o exame.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, desmembrar, desmembrar.fileDCMId, "Validar", erro);
            }
            new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, msg, desmembrar, desmembrar.fileDCMId, $"Validação de desmembrar exame");
            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg Confirmacao(Confirmacao confirmacao)
        {
            List<string> erros = new List<string>();
            var fileDCM = new FileDCMBusiness(_HttpContext).Lista(confirmacao.fileDCMId);
            msg = Validar(confirmacao, fileDCM);
            if (msg.erro == null)
            {
                StatusExameViewModel exame = verificaStatusExame(confirmacao.fileDCMId);

                confirmacao.statusExamesAnterior = exame.statusExames.ToString("g");
                confirmacao.subStatusExamesAnterior = exame.subStatusExames.ToString("g");
                confirmacao.log = new Log().InsertLog(usuarioId);

                if (confirmacao.subStatusExames == SubStatusExames.laudar_segundaLeitura.ToString("g"))
                {
                    confirmacao.segundaLeitura = new Log().InsertLog(usuarioId);
                    confirmacao.log = new Log().InsertLog(usuarioId);
                    confirmacao.log.insertUsuarioId = fileDCM.historicoExame.ListaStatus(StatusExames.laudado).log.insertUsuarioId;
                    confirmacao.log.updateUsuarioId = fileDCM.historicoExame.ListaStatus(StatusExames.laudado).log.updateUsuarioId;
                }
                

                fileDCM.statusExames = confirmacao.statusExames ?? fileDCM.statusExames;
                fileDCM.subStatusExames = confirmacao.subStatusExames ?? fileDCM.subStatusExames;
                fileDCM.prioridade = confirmacao.prioridade ?? fileDCM.prioridade;

                if (fileDCM.historicoExame == null)
                {
                    fileDCM.historicoExame = new List<Confirmacao>();
                }

                if (string.IsNullOrEmpty(confirmacao.templateImpressaoid))
                {
                    confirmacao.templateImpressaoid = fileDCM.TemplateLaudando();
                }

                confirmacao = Particularidades(confirmacao);
                fileDCM.historicoExame.Add(confirmacao);

                if (confirmacao.statusExames == StatusExames.laudar.ToString("g"))
                {
                    fileDCM.dateConfirmacao = DateTime.Now;
                }

                msg = new FileDCMBusiness(_HttpContext).Update(fileDCM);
                if (msg.erro == null)
                {
                    if (confirmacao.desmembrar!= null && confirmacao.desmembrar.confirmacao)
                    {
                        msg = Desmembrar(confirmacao.desmembrar);
                    }

                    if (msg.erro == null)
                    {
                        msg.id = confirmacao.statusExames;
                        if (confirmacao.statusExames != fileDCM.statusExames)
                        {
                            new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, fileDCM, fileDCM.Id, $"Prioridade do Exame alterado para {confirmacao.prioridade}");
                        }
                        new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, fileDCM, fileDCM.Id, $"Status do Exame alterado para {confirmacao.statusExames}", confirmacao.historiaClinica);
                    }

                }
            }

            return new Msg() { erro = List_Erros(erros) };
        }


        internal Msg LogVisualizacao(LogVisualizacao logVisualizacao)
        {
            List<string> erros = new List<string>();

            switch (logVisualizacao.tipo)
            {
                case "laudo":
                    new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, string.Empty, logVisualizacao.fileDCMId, $"Visualização laudo");
                    break;
                case "pdf":
                    new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, string.Empty, logVisualizacao.fileDCMId, $"Realização do download do arquivo laudo em pdf");
                    break;
                case "ohif":
                    new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, string.Empty, logVisualizacao.fileDCMId, $"Visualização de ohif");
                    break;
                case "download":
                    new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, string.Empty, logVisualizacao.fileDCMId, $"Realização do download do arquivo DICOM");
                    break;
                default:
                    erros.Add("Solicitação de log de visualização invalido");
                    new EventoBusiness(_HttpContext).Erro(string.Empty, Telas.Worklist, string.Empty, string.Empty, "Log Visualização", erros.FirstOrDefault());
                    break;
            }

            msg.erro = erros;
            return msg;
        }

        internal Confirmacao Particularidades(Confirmacao confirmacao)
        {
            if (confirmacao.statusExames == StatusExames.laudar.ToString("g"))
            {
                if (confirmacao.subStatusExames == SubStatusExames.laudar_reiterpretacao.ToString("g"))
                {
                    var nota = new FileDCMNota()
                    {
                        fileDCMId = confirmacao.fileDCMId,
                        nota = $"<b>Solicitação de Revisão:</b> {confirmacao.historiaClinica}"
                    };

                    new NotasBusiness(_HttpContext).Update(nota);

                    var histClinico = new FileDCMBusiness(_HttpContext).ListaExame(confirmacao.fileDCMId).historicoExame.ListaStatus(StatusExames.laudar).historiaClinica;
                    confirmacao.historiaClinica = $"{histClinico} {System.Environment.NewLine + System.Environment.NewLine} {nota.nota}";
                }                
            }

            if (confirmacao.subStatusExamesAnterior != SubStatusExames.novo.ToString("g")
                && (confirmacao.subStatusExames == SubStatusExames.novo.ToString("g")))
            {
                confirmacao.subStatusExames = confirmacao.subStatusExamesAnterior;
            }

            return confirmacao;
        }

        internal Msg Validar(Confirmacao confirmacao, FileDCM fileDCM)
        {
            try
            {
                List<string> erros = new List<string>();

                try
                {
                    confirmacao.prioridade.TipoPrioridadeConvert();
                    confirmacao.statusExames.StatusExamesConvert();
                }
                catch (Exception)
                {
                    erros.Add("Erro ao validar o exame.");
                    return new Msg() { erro = List_Erros(erros) };
                }

                var permissoes = new PermissaoBusiness(_HttpContext).List_Permissao_Workilist().ToList();
                var statusExame = confirmacao.statusExames.StatusExamesConvert();

                var facility = new FacilityBusiness(_HttpContext).ListaEaTitle(fileDCM.aeTitle);

                switch (statusExame)
                {
                    case StatusExames.laudando:
                        var permitirLaudo = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, facility.Id, "Laudar imagem");
                        if (!permitirLaudo)
                        {
                            erros.Add("Você não tem permissão para laudar esse exame.");
                            return new Msg() { erro = List_Erros(erros) };
                        }
                        break;
                    case StatusExames.confirmar:
                        var confirmarExame = new PermissaoBusiness(_HttpContext).verificarPermissao(permissoes, facility.Id, "Confirmar exame");
                        if (!confirmarExame)
                        {
                            erros.Add("Você não tem permissão para confirmar esse exame.");
                            return new Msg() { erro = List_Erros(erros) };
                        }
                        break;
                    default:
                        break;
                }

                StatusExameViewModel exame = verificaStatusExame(confirmacao.fileDCMId);

                string criticaAvancaExame = AvancaExame(confirmacao, exame.statusExames, exame.subStatusExames, fileDCM);
                if (!string.IsNullOrEmpty(criticaAvancaExame))
                {
                    erros.Add(criticaAvancaExame);
                }

                return new Msg() { erro = List_Erros(erros) };
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao validar o exame.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, confirmacao, confirmacao.fileDCMId, "Validar Exame", erro);
            }

        }

        internal string AvancaExame(Confirmacao confirmacao, StatusExames statusExameAnterior, SubStatusExames subStatusExameAnterior, FileDCM fileDCM)
        {
            try
            {
                var statusExame = confirmacao.statusExames.StatusExamesConvert();
                var subStatusExame = (confirmacao.subStatusExames.SubStatusExamesConvert());

                //se não tiver alterações de status
                if (statusExame == statusExameAnterior && subStatusExame == subStatusExameAnterior)
                {
                    if (statusExame == StatusExames.laudado && fileDCM.UsuarioLaudando(usuarioId))
                    {
                        var exameInfo = ListaExameInfo(fileDCM.Id, StatusExames.laudado);
                        return $"Esse exame já foi laudado pelo usuário {exameInfo.usuario} ({exameInfo.login}) desde às {exameInfo.data}!";
                    }
                    return string.Empty;
                }
                //verifica se o exame ainda está em transmissão
                if (statusExameAnterior == StatusExames.transmissao)
                {
                    return $"Esse exame ainda está sendo transmitido.";
                }

                //se o exame tiver a confirmar, apenas pode ir para os status de (laudar, desconsiderar e comparação)
                if (statusExameAnterior == StatusExames.confirmar)
                {
                    if (statusExame == StatusExames.laudar || statusExame == StatusExames.desconsiderado || statusExame == StatusExames.comparacao)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return "Não foi possivel avançar fluxo do exame!";
                    }
                }

                if (statusExameAnterior == StatusExames.laudar)
                {
                    switch (statusExame)
                    {
                        case StatusExames.laudando:
                            return string.Empty;
                        case StatusExames.laudar:
                            return string.Empty;
                        case StatusExames.desconsiderado:
                            return string.Empty;
                        case StatusExames.comparacao:
                            return string.Empty;

                        default:
                            break;
                    }
                }

                if (statusExameAnterior == StatusExames.laudando)
                {
                    if (fileDCM.UsuarioLaudando(usuarioId))
                    {
                        switch (statusExame)
                        {
                            case StatusExames.laudar:
                                return string.Empty;
                            case StatusExames.laudado:
                                return string.Empty;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        var exameInfo = ListaExameInfo(fileDCM.Id, StatusExames.laudando);
                        return $"Esse exame está sendo laudado pelo usuário {exameInfo.usuario} ({exameInfo.login}) desde às {exameInfo.data}!";
                    }
                }

                if (statusExameAnterior == StatusExames.laudado)
                {
                    switch (statusExame)
                    {
                        case StatusExames.laudar:
                            if (confirmacao.subStatusExames == SubStatusExames.laudar_reiterpretacao.ToString("g"))
                            {
                                return string.Empty;
                            }
                            break;
                        case StatusExames.laudado:
                            var exameInfo = ListaExameInfo(fileDCM.Id, StatusExames.laudado);
                            return $"Esse exame já foi laudado por {exameInfo.usuario} ({exameInfo.login}) às {exameInfo.data}!";
                        default:
                            break;
                    }
                }

                if (statusExameAnterior == StatusExames.desconsiderado)
                {
                    switch (statusExame)
                    {
                        case StatusExames.laudar:
                            return string.Empty;
                        case StatusExames.comparacao:
                            return string.Empty;
                        default:
                            break;
                    }
                }

                if (statusExameAnterior == StatusExames.comparacao)
                {
                    switch (statusExame)
                    {
                        case StatusExames.laudar:
                            return string.Empty;
                        case StatusExames.desconsiderado:
                            return string.Empty;
                        default:
                            break;
                    }
                }

                return "Não foi possivel avançar fluxo do exame!";
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao avançar fluxo do exame.";
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, confirmacao, confirmacao.fileDCMId, "AvancaExame", erro);
                return erro;
            }
        }

        internal NotaHistoricoViewModel HistoricoClinicoLoad(string fileId)
        {
            try
            {
                List<string> erros = new List<string>();

                var confirmacao = new FileDCMBusiness(_HttpContext).ListaExame(fileId).historicoExame.ListaStatus(StatusExames.laudar);
                var usuario = new UsuarioBusiness(_HttpContext).UsuarioView(confirmacao.log.insertUsuarioId);

                return new NotaHistoricoViewModel()
                {
                    nota = confirmacao.historiaClinica,
                    usuarioId = usuario.Id,
                    perfil = usuario.perfil,
                    usuario = usuario.nome,
                    data = confirmacao.log.insertData
                };
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao buscar historico clinico.";
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Worklist, string.Empty, fileId, "Exame", erro);
                return new NotaHistoricoViewModel();
            }

        }

    }
}
