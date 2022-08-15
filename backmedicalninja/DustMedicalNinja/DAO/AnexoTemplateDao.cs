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
    public class AnexoTemplateDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(AnexoTemplate anexoTemplate)
        {
            await _ConexaoMongoDB.AnexoTemplate.InsertOneAsync(anexoTemplate);
            return anexoTemplate.Id;
        }

        internal async void Update(AnexoTemplate anexoTemplate
            )
        {
            var condicao = Builders<AnexoTemplate>.Filter.Eq(x => x.Id, anexoTemplate.Id);
            await _ConexaoMongoDB.AnexoTemplate.ReplaceOneAsync(condicao, anexoTemplate);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.AnexoTemplate.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<AnexoTemplate>> List(string empresaId)
        {
            var condicao = Builders<AnexoTemplate>.Filter.Eq(x => x.empresaId, empresaId);
            var listaAnexoTemplate = await _ConexaoMongoDB.AnexoTemplate.Find(condicao).ToListAsync();

            return listaAnexoTemplate;
        }

        internal async Task<AnexoTemplate> ListAnexoTemplate(string anexoTemplateId)
        {
            var condicao = Builders<AnexoTemplate>.Filter.Eq(x => x.Id, anexoTemplateId);
            var anexoTemplate = await _ConexaoMongoDB.AnexoTemplate.Find(condicao).FirstOrDefaultAsync();

            return anexoTemplate;
        }

        internal async Task<long> CountAnexoTemplate(string empresaId)
        {
            var condicao = Builders<AnexoTemplate>.Filter.Eq(x => x.empresaId, empresaId);
            var countAnexoTemplate = await _ConexaoMongoDB.AnexoTemplate.Find(condicao).CountDocumentsAsync();

            return countAnexoTemplate;
        }
    }
}
