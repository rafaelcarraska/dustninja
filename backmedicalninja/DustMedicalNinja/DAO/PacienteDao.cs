using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.DAO
{
    public class PacienteDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Paciente paciente)
        {
            await _ConexaoMongoDB.Paciente.InsertOneAsync(paciente);
            return paciente.Id;
        }

        internal void InsertList(List<Paciente> list_paciente)
        {
            _ConexaoMongoDB.Paciente.InsertMany(list_paciente);
        }

        internal async void InsertListAsysnc(List<Paciente> list_paciente)
        {
            await _ConexaoMongoDB.Paciente.InsertManyAsync(list_paciente);
        }

        internal async void Update(Paciente paciente)
        {
            var condicao = Builders<Paciente>.Filter.Eq(x => x.Id, paciente.Id);
            await _ConexaoMongoDB.Paciente.ReplaceOneAsync(condicao, paciente);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.Paciente.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<Paciente>> ListAll(string empresaId)
        {
            var condicao = Builders<Paciente>.Filter.Eq(x => x.empresaId, empresaId);
            List<Paciente> list_paciente = await _ConexaoMongoDB.Paciente.Find(condicao)
                .ToListAsync();

            return list_paciente;
        }

        internal async Task<List<Paciente>> ListFileDCM(List<string> listaPaciente)
        {
            List<Paciente> list_paciente = await _ConexaoMongoDB.Paciente.Find(x => listaPaciente.Contains(x.Id))
                .ToListAsync();

            return list_paciente;
        }

        internal async Task<List<Paciente>> ListaFacility(string aeTitle)
        {
            var listaPaciente = _ConexaoMongoDB.FileDCM.Find(x => x.aeTitle == aeTitle).ToList().Select(x => x.pacienteId);
            List<Paciente> list_paciente = await _ConexaoMongoDB.Paciente.Find(x => x.status == true && listaPaciente.Contains(x.Id))
                .ToListAsync();

            return list_paciente;
        }

        internal async Task<List<Paciente>> ListCombo()
        {
            List<Paciente> list_paciente = await _ConexaoMongoDB.Paciente.Find(x => 
                x.status == true
            ).ToListAsync();

            return list_paciente;
        }

        internal async Task<List<Paciente>> List(string pacienteId)
        {
            var condicao = Builders<Paciente>.Filter.Eq(x => x.Id, pacienteId);
            var lista = await _ConexaoMongoDB.Paciente.Find(condicao).ToListAsync();

            return lista;
        }

        internal async Task<List<Paciente>> ListPk(int pkPostgre)
        {
            var condicao = Builders<Paciente>.Filter.Eq(x => x.pkPostgre, pkPostgre);
            var lista = await _ConexaoMongoDB.Paciente.Find(condicao).ToListAsync();

            return lista;
        }

        internal async Task<long> ExisteNome(Paciente paciente)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(paciente.Id))
                {
                    qtd = await _ConexaoMongoDB.Paciente.Find(x => 
                    x.Id != paciente.Id &&
                    x.nome == paciente.nome).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.Paciente.Find(x =>
                    x.nome == paciente.nome).CountDocumentsAsync();
                }                                

                return qtd;
            }
            catch (Exception ex)
            {
                string er = ex.Message;
                throw;
            }

        }        
    }
}
