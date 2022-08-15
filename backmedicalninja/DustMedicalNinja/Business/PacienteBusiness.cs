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
using DustMedicalNinja.Models.Postgre;
using DustMedicalNinja.Context;
using DustMedicalNinja.Models.ViewModel;

namespace DustMedicalNinja.Business
{
    internal class PacienteBusiness : Uteis
    {
        PacienteDao _PacienteDao = new PacienteDao();
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;
        private string usuarioId;
        private bool master;

        internal PacienteBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            usuarioId = _HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
            master = Convert.ToBoolean(_HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.GroupSid).Select(c => c.Value).SingleOrDefault());
        }

        internal Msg Insert(Paciente paciente)
        {
            msg = new Msg();
            try
            {
                paciente.log = new Log().InsertLog(usuarioId);
                msg.id = _PacienteDao.Insert(paciente).Result;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o paciente: {paciente.nome}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Paciente, paciente, paciente.Id, "Insert", erro);
            }
        }

        internal void InsertList(List<Paciente> listPaciente)
        {
            if (listPaciente.Count > 0)
            {
                _PacienteDao.InsertList(listPaciente);
            }
        }

        internal IEnumerable<PacienteViewModel> ListAllViewModel()
        {
            var paciente = _PacienteDao.ListAll(empresaId).Result;
            var pacienteCount = new FileDCMBusiness(_HttpContext).CountPacienteExames(empresaId);
            var facility = new FacilityBusiness(_HttpContext).ListAll();
            var result = (from r in paciente
                          join count in pacienteCount on r.Id equals count.pacienteId into pcount
                          from subpact in pcount.DefaultIfEmpty()
                          orderby r.nomeCompleto, r.dataNascimento
                          select new PacienteViewModel()
                          {
                              Id = r.Id,
                              log = r.log,
                              empresaId = r.empresaId,
                              status = r.status,
                              pkPostgre = r.pkPostgre,
                              dataNascimento = r.dataNascimento,
                              nome = r.nome,
                              namePrefix = r.namePrefix,
                              middleName = r.middleName,
                              giveName = r.giveName,
                              pacienteIdDCM = r.pacienteIdDCM,
                              sexo = r.sexo,
                              aeTitle = subpact?.aeTitle ?? "",
                              facility = facility.FirstOrDefault(x => x.aeTitle == (subpact?.aeTitle ?? ""))?.descricao ?? "",
                              countExames = subpact?.countExames ?? 0
                          });

            return result.ToList();
        }

        internal IEnumerable<PacienteViewModel> ListaFacility(string facilityId)
        {
            var facility = new FacilityBusiness(_HttpContext).Lista(facilityId);

            var paciente = _PacienteDao.ListaFacility(facility.aeTitle).Result;
            var pacienteCount = new FileDCMBusiness(_HttpContext).CountPacienteExames(empresaId);
            var result = (from r in paciente
                          join count in pacienteCount on r.Id equals count.pacienteId into pcount
                          from subpact in pcount.DefaultIfEmpty()
                          orderby r.nomeCompleto, r.dataNascimento
                          select new PacienteViewModel()
                          {
                              Id = r.Id,
                              log = r.log,
                              empresaId = r.empresaId,
                              status = r.status,
                              pkPostgre = r.pkPostgre,
                              dataNascimento = r.dataNascimento,
                              nome = r.nome,
                              namePrefix = r.namePrefix,
                              middleName = r.middleName,
                              giveName = r.giveName,
                              pacienteIdDCM = r.pacienteIdDCM,
                              sexo = r.sexo,
                              aeTitle = subpact?.aeTitle ?? "",
                              facility = facility?.descricao ?? "",
                              countExames = subpact?.countExames ?? 0
                          });

            return result.ToList();
        }

        internal IEnumerable<Paciente> ListAll()
        {
            var result = _PacienteDao.ListAll(empresaId).Result;
            return from r in result
                   orderby r.status descending, r.nome
                   select r;
        }

        internal IEnumerable<Combobox> ListaCombo()
        {
            var result = _PacienteDao.ListCombo().Result;
            return from r in result
                   orderby r.nome
                   select new Combobox()
                   {
                       Id = r.Id,
                       descricao = r.nome
                   };
        }

        internal Paciente Lista(string Id)
        {
            List<Paciente> listaPaciente = _PacienteDao.List(Id).Result;
            if (listaPaciente.Count > 0)
            {
                return listaPaciente.First();
            }
            return new Paciente();
        }

        internal PacienteViewModel ListaViewModel(string Id)
        {
            List<Paciente> listaPaciente = _PacienteDao.List(Id).Result;
            if (listaPaciente.Count > 0)
            {
                var paciente = listaPaciente.First();
                var pacienteViewModel = new PacienteViewModel()
                {
                    Id = paciente.Id,
                    log = paciente.log,
                    empresaId = paciente.empresaId,
                    status = paciente.status,
                    pkPostgre = paciente.pkPostgre,
                    dataNascimento = paciente.dataNascimento,
                    nome = paciente.nome,
                    namePrefix = paciente.namePrefix,
                    middleName = paciente.middleName,
                    giveName = paciente.giveName,
                    pacienteIdDCM = paciente.pacienteIdDCM,
                    sexo = paciente.sexo,
                    countExames = (int)new FileDCMBusiness(_HttpContext).CountPacienteId(paciente.Id)
                };
                return pacienteViewModel;
            }
            return new PacienteViewModel();
        }

        internal Paciente ListaPK(int Id)
        {
            List<Paciente> listaPaciente = _PacienteDao.ListPk(Id).Result;
            if (listaPaciente.Count > 0)
            {
                return listaPaciente.First();
            }
            return null;
        }

        internal string SalvarSync(Patient paciente, PersonName personName, PatientId patId)
        {
            Paciente _paciente = ListaPK(paciente.pk);
            if (_paciente == null)
            {
                _paciente = new Paciente()
                {
                    nome = (personName == null ? string.Empty : personName.family_name),
                    giveName = (personName == null ? string.Empty : personName.given_name),
                    middleName = (personName == null ? string.Empty : personName.middle_name),
                    namePrefix = (personName == null ? string.Empty : personName.name_prefix),
                    sexo = paciente.pat_sex,
                    dataNascimento = paciente.pat_birthdate.StringToDate(),
                    pkPostgre = paciente.pk,
                    pacienteIdDCM = (patId == null ? string.Empty : patId.pat_id),
                    status = true,
                    empresaId = empresaId,
                    log = new Log().InsertLog(usuarioId)
                };

                return Insert(_paciente).id;
            }

            return _paciente.Id;
        }

        internal Msg Salvar(Paciente paciente, DCMContext _context)
        {
            msg = Validar(paciente);
            if (msg.erro == null)
            {
                if (string.IsNullOrEmpty(paciente.Id))
                {
                    List<string> erros = new List<string>();
                    erros.Add("Não é possivel criar paciente !");
                    return new Msg() { erro = List_Erros(erros) };
                }

                return Update(paciente, _context);
            }

            return msg;
        }

        internal Msg TrocaPaciente(TrocaPacienteViewModel trocaPaciente, DCMContext _context)
        {

            if (string.IsNullOrEmpty(trocaPaciente.fileDCMId))
            {
                List<string> erros = new List<string>();
                erros.Add("Erro ao trocar o paciente !");
                new EventoBusiness(_HttpContext).Sucesso(Telas.Paciente, "", trocaPaciente, trocaPaciente.fileDCMId, "Troca", "Erro ao trocar o paciente !");
                return new Msg() { erro = List_Erros(erros) };
            }
            var fileDCM = new FileDCMBusiness(_HttpContext).Lista(trocaPaciente.fileDCMId);
            var pacienteAntigo = new PacienteBusiness(_HttpContext).Lista(fileDCM.pacienteId).nomeCompleto;
            var pacienteNovo = new PacienteBusiness(_HttpContext).Lista(trocaPaciente.pacienteId).nomeCompleto;

            msg = ValidarTrocaPaciente(trocaPaciente, fileDCM.aeTitle);
            if (msg.erro == null)
            {
                fileDCM.pacienteId = trocaPaciente.pacienteId;
                fileDCM.log = fileDCM.log.UpdateLog(usuarioId);

                msg = new FileDCMBusiness(_HttpContext).UpdateSync(fileDCM, _context);
            }
            if (msg.erro != null)
            {
                new EventoBusiness(_HttpContext).Sucesso(Telas.Paciente, "", trocaPaciente, trocaPaciente.fileDCMId, "Troca", "Erro ao trocar o paciente !");
            }
            else
            {
                new EventoBusiness(_HttpContext).Sucesso(Telas.Worklist, string.Empty, string.Empty, trocaPaciente.fileDCMId, $"Paciente aterado de:{pacienteAntigo} / para:{pacienteNovo}");
            }

            return msg;
        }

        internal Msg Update(Paciente paciente, DCMContext _context = null)
        {
            msg = new Msg();
            try
            {
                var pacienteAntigo = Lista(paciente.Id);

                paciente.empresaId = pacienteAntigo.empresaId;
                paciente.pkPostgre = pacienteAntigo.pkPostgre;
                paciente.pacienteIdDCM = pacienteAntigo.pacienteIdDCM;

                paciente.log = paciente.log.UpdateLog(usuarioId);
                _PacienteDao.Update(paciente);

                if (_context != null)
                {
                    UpdateDCM(paciente, _context);
                }

                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o paciente: {paciente.nome}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Paciente, paciente, paciente.Id, "Update", erro);
            }
        }

        internal Msg UpdateDCM(Paciente paciente, DCMContext _context)
        {
            msg = new Msg();
            try
            {
                var patientId = _context.Patient.FirstOrDefault(x => x.pk == paciente.pkPostgre);
                var personName = _context.PersonName.FirstOrDefault(x => x.pk == patientId.pat_name_fk);

                personName.family_name = paciente.nome;
                personName.given_name = paciente.giveName;
                personName.middle_name = paciente.middleName;
                personName.name_prefix = paciente.namePrefix;
                patientId.pat_sex = paciente.sexo;
                patientId.pat_birthdate = paciente.dataNascimento.ToString("yyyyMMdd");

                _context.SaveChanges();
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao editar o paciente DCM: {paciente.nome}.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Paciente, paciente, paciente.Id, "Update", erro);
            }
        }

        internal Msg Validar(Paciente paciente)
        {
            List<string> erros = new List<string>();

            if (!_PacienteDao.ExisteNome(paciente).Result.Equals(0))
                erros.Add("Essa nome paciente já existe!");

            if (!master && paciente.status != Lista(paciente.Id).status)
            {
                erros.Add("Apenas usuário master pode alterar o status para inativo!");
            }

            return new Msg() { erro = List_Erros(erros) };
        }

        internal Msg ValidarTrocaPaciente(TrocaPacienteViewModel trocaPaciente, string aeTitle)
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(trocaPaciente.fileDCMId) || string.IsNullOrEmpty(trocaPaciente.pacienteId))
            {
                erros.Add("Erro ao validar o processo!");
            }
            else
            {
                var facility = new FacilityBusiness(_HttpContext).ListaEaTitle(aeTitle);
                var listaPaciente = ListaFacility(facility.Id).ToList();

                if (!listaPaciente.Select(x => x.aeTitle).Contains(aeTitle))
                    erros.Add("Erro ao validar o processo!");
            }

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
                    _PacienteDao.Delete(Id);
                    return msg;
                }
                var msgInativar = Inativar(Id);
                if (string.IsNullOrEmpty(msgInativar))
                {
                    msg.erro.Add($"Não é possivel deletar esse paciente, pois ele está em uso.");
                    msg.erro.Add($"Status do paciente alterado para Inativo.");
                }
                listaErro.Add(msgInativar);

                msg.erro = listaErro;
                msg.status = false;
                return msg;
            }
            catch (Exception ex)
            {
                string erro = $"Erro ao deletar o paciente.";
                return new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Paciente, string.Empty, Id, "Delete", erro);
            }
        }

        internal string Inativar(string Id)
        {
            try
            {
                Paciente paciente = Lista(Id);

                if (paciente.status)
                {
                    paciente.status = false;
                    Update(paciente);
                }

                return null;
            }
            catch (Exception)
            {
                return $"Erro ao inativar o paciente.";
            }
        }

        internal bool EmUso(string Id)
        {
            if (new FileDCMBusiness(_HttpContext).CountPacienteId(Id) > 0)
            {
                return true;
            }
            return false;
        }

        internal bool Input(int quantidade)
        {
            try
            {
                var listPaciente = new List<Paciente>();

                for (int x = 0; x < quantidade; x++)
                {
                    var paciente = new Paciente
                    {
                        nome = "paciente" + Aleatorio(10, 2),
                        status = (new Random().Next(0, 2) == 0),
                        log = new Log().InsertLog(usuarioId)
                    };

                    listPaciente.Add(paciente);
                }

                _PacienteDao.InsertList(listPaciente);
                new EventoBusiness(_HttpContext).Sucesso(Telas.Empresa, "", listPaciente.Count(), string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return true;
            }
            catch (Exception ex)
            {
                new EventoBusiness(_HttpContext).Erro(ex.Message, Telas.Paciente, string.Empty, string.Empty, "Input", "quantidade solicitada: " + quantidade);
                return false;
            }

        }


    }
}
