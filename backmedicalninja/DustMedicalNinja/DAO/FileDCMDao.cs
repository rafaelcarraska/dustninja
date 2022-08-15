using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Models.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.DAO
{
    public class FileDCMDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(FileDCM filedcm)
        {
            await _ConexaoMongoDB.FileDCM.InsertOneAsync(filedcm);
            return filedcm.Id;
        }

        internal List<FileDCM> Insert_List(List<FileDCM> list_filedcm)
        {
            _ConexaoMongoDB.FileDCM.InsertManyAsync(list_filedcm);
            return list_filedcm;
        }

        internal async void Update(FileDCM filedcm)
        {
            var condicao = Builders<FileDCM>.Filter.Eq(x => x.Id, filedcm.Id);
            await _ConexaoMongoDB.FileDCM.ReplaceOneAsync(condicao, filedcm);
        }

        internal void UpdateSync(FileDCM filedcm)
        {
            var condicao = Builders<FileDCM>.Filter.Eq(x => x.Id, filedcm.Id);
            _ConexaoMongoDB.FileDCM.ReplaceOneAsync(condicao, filedcm);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.FileDCM.DeleteOneAsync(x => x.Id == Id);
        }

        //internal async Task<List<FileDCM>> List_All()
        //{
        //    var condicao = Builders<FileDCM>.Filter.Eq(x => x.status, true);
        //    var list_filedcm = await _ConexaoMongoDB.FileDCM.Find(condicao).ToListAsync();

        //    return list_filedcm;
        //}

        internal async Task<List<FileDCM>> ListModalidadeVazia()
        {
            var list_filedcm = await _ConexaoMongoDB.FileDCM
                .Find(x => string.IsNullOrEmpty(x.modality) && x.data_envio >= DateTime.Now.AddDays(-30) && x.status == true)
                .ToListAsync();

            return list_filedcm;
        }

        internal async Task<List<FileDCM>> List_All(string empresaId)
        {
            var list_filedcm = await _ConexaoMongoDB.FileDCM
                .Find(x => x.empresaId == empresaId && x.status == true).ToListAsync();

            return list_filedcm;
        }

        internal async Task<FileDCM> List(string Id)
        {
            var condicao = Builders<FileDCM>.Filter.Eq(x => x.Id, Id);
            var fileDCM = await _ConexaoMongoDB.FileDCM.Find(condicao).FirstOrDefaultAsync();

            return fileDCM;
        }

        internal async Task<List<FileDCM>> List_StudyID(FileDCM filedcm)
        {
            var condicao = Builders<FileDCM>.Filter.Eq(x => x.studyId, filedcm.studyId);
            var lista = await _ConexaoMongoDB
                .FileDCM.Find(x => x.studyId == filedcm.studyId && x.status == true).ToListAsync();
            return lista;
        }

        internal async Task<List<FileDCM>> ListaPaciente(string pacienteId)
        {
            var list_fileDCMPaciente = await _ConexaoMongoDB.FileDCM
                .Find(x => x.pacienteId == pacienteId && x.status == true)
                .ToListAsync();

            return list_fileDCMPaciente;
        }

        internal async Task<long> ListaPacienteCount(string pacienteId)
        {
            var list_fileDCMPaciente = await _ConexaoMongoDB.FileDCM
                .Find(x => x.pacienteId == pacienteId && x.status == true)
                .CountDocumentsAsync();

            return list_fileDCMPaciente;
        }

        internal async Task<List<PacienteCountExamesViewModel>> CountPacienteExames(string empresaId)
        {
            var query = from p in _ConexaoMongoDB.FileDCM.AsQueryable()
                        where p.empresaId == empresaId && p.status == true
                        group p by new { p.pacienteId, p.aeTitle } into g
                        select new PacienteCountExamesViewModel {
                            pacienteId = g.Key.pacienteId,
                            aeTitle = g.Key.aeTitle,
                            countExames = g.Count()
                        };

            return query.ToList();
        }        

        internal async Task<long> ExisteExames(string aeTitle)
        {
            try
            {
                long qtd;
                    qtd = await _ConexaoMongoDB.FileDCM.Find(x =>
                    x.aeTitle == aeTitle && x.status == true).CountDocumentsAsync();                

                return qtd;
            }
            catch (Exception ex)
            {
                string er = ex.Message;
                return 1;
            }

        }
    }
}
