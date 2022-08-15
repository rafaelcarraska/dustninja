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
using System.IO;

namespace DustMedicalNinja.Business
{
    internal class RelatorioBusiness : Uteis
    {
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;

        internal RelatorioBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
        }

        internal byte[] getRelatorioGerencial(RelatorioCSV relatorioCSV)
        {
            new EventoBusiness(_HttpContext).Sucesso(Telas.Relatorio, string.Empty, relatorioCSV, string.Empty, "Relatorio Geral", string.Empty, false);
            byte[] bytes = null;

            var listaFileDCM = new FileDCMBusiness(_HttpContext).Carregar_DCM4CHEE();

            
                if (relatorioCSV.de != null && relatorioCSV.de > DateTime.MinValue)
                {
                    listaFileDCM = listaFileDCM.Where(x => x.date_envio >= relatorioCSV.de).ToList();
                }
                if (relatorioCSV.ate != null && relatorioCSV.ate > DateTime.MinValue)
                {
                    if (relatorioCSV.de <= relatorioCSV.ate)
                    {
                        listaFileDCM = listaFileDCM.Where(x => x.date_envio <= relatorioCSV.ate).ToList();
                    }
                }


            using (var ms = new MemoryStream())
            {
                TextWriter tw = new StreamWriter(ms);
                var linha = new List<string>();
                linha.Add("Instituição");
                linha.Add("Nome do paciente");
                linha.Add("Id do paciente");
                linha.Add("Modalidade");
                linha.Add("Unidade");
                linha.Add("Descrição do estudo");
                linha.Add("Data de nascimento do paciente");
                linha.Add("Data de envio");
                linha.Add("data de Confirmação");
                linha.Add("Id do estudo");
                linha.Add("Status do exame");
                linha.Add("Sub status do exame");
                linha.Add("TLP");
                linha.Add("Prioridade");
                linha.Add("body_part");
                linha.Add("aeTitle");
                linha.Add("Usuário Confirmação");
                linha.Add("Login Confirmação");
                linha.Add("Data de Confirmação");
                linha.Add("Usuário laudo");
                linha.Add("Login laudo");
                linha.Add("Data do laudo");
                linha.Add("Tipo de Estudo");
                tw.WriteLine(string.Join(relatorioCSV.separador, linha));

                var historicoClinico = new List<Confirmacao>();
                var listaTipoEstudo = new TipoExameBusiness(_HttpContext).ListAll();

                foreach (var fileDCM in listaFileDCM)
                {
                    try
                    {
                        historicoClinico = new List<Confirmacao>();
                        historicoClinico = new FileDCMBusiness(_HttpContext).ListaExame(fileDCM.Id).historicoExame;
                        var usuarios = new UsuarioBusiness(_HttpContext).ListAll();

                        linha = new List<string>();
                        linha.Add((fileDCM.institution ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.pacienteNome ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.paciente.pacienteIdDCM ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.modality ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.facilityDesc ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.studyDesc ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.paciente.dataNascimento_formatada ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.data_envio_formatada ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.dateConfirmacaoFormatada ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.studyId ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.statusExamesFormatado ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.subStatusExamesFormatado ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.tempoParaLaudar ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.prioridadeFormatada ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.body_part ?? "").Replace(relatorioCSV.separador, ""));
                        linha.Add((fileDCM.aeTitle ?? "").Replace(relatorioCSV.separador, ""));

                        if (historicoClinico != null)
                        {
                            linha.Add(CarregaNome(usuarios, StatusExames.laudar, historicoClinico).Replace(relatorioCSV.separador, ""));
                            linha.Add(CarregaLogin(usuarios, StatusExames.laudar, historicoClinico).Replace(relatorioCSV.separador, ""));
                            linha.Add(CarregaData(usuarios, StatusExames.laudar, historicoClinico).Replace(relatorioCSV.separador, ""));
                            linha.Add(CarregaNome(usuarios, StatusExames.laudado, historicoClinico).Replace(relatorioCSV.separador, ""));
                            linha.Add(CarregaLogin(usuarios, StatusExames.laudado, historicoClinico).Replace(relatorioCSV.separador, ""));
                            linha.Add(CarregaData(usuarios, StatusExames.laudado, historicoClinico).Replace(relatorioCSV.separador, ""));
                            linha.Add(CarregaEstudo(usuarios, StatusExames.laudado, historicoClinico, listaTipoEstudo).Replace(relatorioCSV.separador, ""));
                        }

                        tw.WriteLine(string.Join(relatorioCSV.separador, linha));
                    }
                    catch (Exception ex)
                    {
                        var msg = "Erro ao criar a linha do registro.";
                        tw.WriteLine(msg);
                        new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Relatorio, relatorioCSV, string.Empty, "Listar", msg);
                    }
                    
                }
                tw.Flush();
                ms.Position = 0;
                bytes = ms.ToArray();
            }

            return bytes;
        }

        internal string CarregaNome(IEnumerable<Usuario> usuarios, StatusExames status, List<Confirmacao> historicoClinico)
        {
            var histClinico = historicoClinico.ListaStatus(status);
            if (histClinico != null)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id == histClinico.log.insertUsuarioId);
                if (usuario != null)
                {
                    return usuario.nome;
                }
            }
            return string.Empty;
        }

        internal string CarregaLogin(IEnumerable<Usuario> usuarios, StatusExames status, List<Confirmacao> historicoClinico)
        {
            var histClinico = historicoClinico.ListaStatus(status);
            if (histClinico != null)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id == histClinico.log.insertUsuarioId);
                if (usuario != null)
                {
                    return usuario.login;
                }
            }
            return string.Empty;
        }


        internal string CarregaData(IEnumerable<Usuario> usuarios, StatusExames status, List<Confirmacao> historicoClinico)
        {
            var histClinico = historicoClinico.ListaStatus(status);
            if (histClinico != null)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id == histClinico.log.insertUsuarioId);
                if (usuario != null)
                {
                    return histClinico.log.insertDataFormatada;
                }
            }
            return string.Empty;
        }

        internal string CarregaEstudo(IEnumerable<Usuario> usuarios, StatusExames status, List<Confirmacao> historicoClinico, List<TipoExame> listaTipoEstudo)
        {
            var result = string.Empty;
            var histClinico = historicoClinico.ListaStatus(status);
            if (histClinico != null)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id == histClinico.log.insertUsuarioId);
                if (usuario != null)
                {
                    var estudosSelecionados = listaTipoEstudo.Where(x => histClinico.listaTipoEstudo.Contains(x.Id)).ToList();

                    foreach (var TipoEstudo in estudosSelecionados)
                    {
                        result += $"({TipoEstudo.nome} - {TipoEstudo.modalidade})";
                    }
                }
            }
            return result;
        }

    }
}
