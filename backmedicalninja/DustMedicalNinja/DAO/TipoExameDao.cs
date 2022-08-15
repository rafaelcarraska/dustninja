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
    public class TipoExameDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(TipoExame tipoExame)
        {
            await _ConexaoMongoDB.TipoExame.InsertOneAsync(tipoExame);
            return tipoExame.Id;
        }

        internal async void InsertList(List<TipoExame> listTipoExame)
        {
            await _ConexaoMongoDB.TipoExame.InsertManyAsync(listTipoExame);
        }

        internal async void Update(TipoExame tipoExame)
        {
            var condicao = Builders<TipoExame>.Filter.Eq(x => x.Id, tipoExame.Id);
            await _ConexaoMongoDB.TipoExame.ReplaceOneAsync(condicao, tipoExame);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.TipoExame.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<TipoExame>> ListAll(string empresaId)
        {
            var byEmpresa = Builders<TipoExame>.Filter.Eq(x => x.empresaId, empresaId);
            List<TipoExame> listTipoExame = await _ConexaoMongoDB.TipoExame.Find(byEmpresa).ToListAsync();

            return listTipoExame;
        }

        internal async Task<List<TipoExame>> List(string tipoExameId)
        {
            var condicao = Builders<TipoExame>.Filter.Eq(x => x.Id, tipoExameId);
            var lista = await _ConexaoMongoDB.TipoExame.Find(condicao).ToListAsync();

            return lista;
        }

        internal async Task<List<TipoExame>> ListCombo(string empresaId)
        {
            List<TipoExame> list_tipoExame = await _ConexaoMongoDB.TipoExame.Find(x =>
                x.status == true && x.empresaId == empresaId
            ).ToListAsync();

            return list_tipoExame;
        }

        internal async Task<List<TipoExame>> listaTipoEstudo(string empresaId)
        {
            List<TipoExame> list_tipoExame = await _ConexaoMongoDB.TipoExame.Find(x =>
                x.status == true && x.empresaId == empresaId
            ).ToListAsync();

            return list_tipoExame;
        }

        internal async Task<long> ExisteDescricao(TipoExame tipoExame)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(tipoExame.Id))
                {
                    qtd = await _ConexaoMongoDB.TipoExame.Find(x => 
                    x.Id != tipoExame.Id &&
                    x.empresaId == tipoExame.empresaId &&
                    x.nome == tipoExame.nome).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.TipoExame.Find(x =>
                    x.empresaId == tipoExame.empresaId &&
                    x.nome == tipoExame.nome).CountDocumentsAsync();
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
