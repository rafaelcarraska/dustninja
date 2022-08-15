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
    public class ConfiguracaoDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Configuracao configuracao)
        {
            await _ConexaoMongoDB.Configuracao.InsertOneAsync(configuracao);
            return configuracao.Id;
        }

        internal async void Update(Configuracao configuracao)
        {
            var condicao = Builders<Configuracao>.Filter.Eq(x => x.Id, configuracao.Id);
            await _ConexaoMongoDB.Configuracao.ReplaceOneAsync(condicao, configuracao);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.Configuracao.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<Configuracao> List(string usuarioId)
        {
            var condicao = Builders<Configuracao>.Filter.Eq(x => x.usuarioId, usuarioId);
            var configuracao = await _ConexaoMongoDB.Configuracao.Find(condicao).FirstOrDefaultAsync();

            return configuracao;
        }
    }
}
